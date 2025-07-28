using System;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Components.Transition
{
    /// <summary>
    /// Component stores default sprites of novel Places.
    /// Changes sprite depending on requested state from Place class using Location Manager.
    /// Handle custom states (sprites) by using additional component on prefab (CustomPlaceStatesComponent)
    /// When we enter location -> performs sprite load without fade base on place state condition. Fade effect can be invoked by using event with (setWithFade=true) option. It will perform realtime sprite change animation with fade effect.
    /// todo if sprite will be the same after change/split - ignore operation
    /// </summary>
    public class NovelViewPlaceStateControllerComponent : MonoBehaviour
    {
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int OldTex = Shader.PropertyToID("_OldTex");
        private static readonly int Fade = Shader.PropertyToID("_Fade");
        [SerializeField] private string currentPlaceName;
        
        private CustomPlaceStatesComponent _customPlaceStatesComponent;
        
        [FormerlySerializedAs("placeStateSprites")] [FormerlySerializedAs("roomStateSprites")] [SerializeField] private List<Sprite> defaultStateSprites;
        
        private SpriteRenderer _spriteRenderer;
        
        private bool _isInCutscene;

        //for shader graph sprite transition
        private Material _material;
        private Texture2D _oldTexture;
        
        //dotween
        private Tween _tween;
        private readonly float _fadeDuration=1f;

        private void OnEnable()
        {
            EventManager.Instance.LocationsAndPlacesEvents.OnUpdatePlaceStateSprite += UpdatePlaceStateSprite;
            EventManager.Instance.LocationsAndPlacesEvents.OnGetCurrentPlaceSprite += GetCurrentPlaceSprite;
            
            EventManager.Instance.TimeEvents.OnTimeChange += OnTimeChange;
            EventManager.Instance.CutsceneEvents.OnCutsceneStarted += OnCutsceneStarted;
            EventManager.Instance.CutsceneEvents.OnCutsceneFinished += OnCutsceneFinished;
        }

        private void OnDisable()
        {
            EventManager.Instance.LocationsAndPlacesEvents.OnUpdatePlaceStateSprite -= UpdatePlaceStateSprite;
            EventManager.Instance.LocationsAndPlacesEvents.OnGetCurrentPlaceSprite -= GetCurrentPlaceSprite;
            
            EventManager.Instance.TimeEvents.OnTimeChange -= OnTimeChange;
            EventManager.Instance.CutsceneEvents.OnCutsceneStarted -= OnCutsceneStarted;
            EventManager.Instance.CutsceneEvents.OnCutsceneFinished -= OnCutsceneFinished;
        }

        private void OnDestroy()
        {
            KillCurrentTween();
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            currentPlaceName = gameObject.name.Replace("(Clone)", ""); //get name from place prefab and set as currentPlaceName
            
            
            if (LocationManager.Instance == null)
            {
                Debug.LogError("NovelViewPlaceStateControllerComponent can't find LocationManager");
                return;
            }

            _customPlaceStatesComponent = GetComponent<CustomPlaceStatesComponent>();
            
            //get appropriate sprite for place on start (when we enter location)
            SetPlaceStateSprite(false);
            EventManager.Instance.LocationsAndPlacesEvents.PlaceSpriteChanged(_spriteRenderer);
            
            //for shader graph sprite transition
            _material = _spriteRenderer.material;
            if (!_material.name.Contains("SpriteFadeMat"))
            {
                Debug.LogError($"Sprite renderer for {currentPlaceName} contains invalid shader. Must be SpriteFadeMat but is currently {_material.name}");
            }
            _oldTexture = _spriteRenderer.sprite.texture;
        }

        //cutscenes etc must be handled
        private void OnTimeChange(string timeString)
        {
            if (_isInCutscene)
            {
                return;
            }
            SetPlaceStateSprite(true);
        }
        private void OnCutsceneStarted()
        {
            _isInCutscene = true;
        }
        private void OnCutsceneFinished()
        {
            _isInCutscene = false;
        }
        
        /// <summary>
        /// ///Method gets place state. Base on it and fade option perform default or custom sprite change using sprites from components.
        /// </summary>
        /// <param name="setWithFade"></param>
        private void SetPlaceStateSprite(bool setWithFade)
        {
            var locationManager = LocationManager.Instance;
            TimeOfDayEnum timeOfDay = locationManager.GetLocationTimeOfDayState(currentPlaceName);
            string placeState = locationManager.GetPlaceState(currentPlaceName).ToString();
    
            if (placeState == "Default") //place enum = default
            {
                SetDefaultStateSprite(timeOfDay, setWithFade);
            }
            else
            {
                if (_customPlaceStatesComponent != null)
                {
                    var customSprite = _customPlaceStatesComponent.GetCustomPlaceStateSprite(timeOfDay, placeState);
                    if (!setWithFade)
                    {
                        _spriteRenderer.sprite = customSprite;
                    }
                    else
                    {
                        PerformSpriteTransitionUsingShader(customSprite);
                    }
                }
                else
                {
                    Debug.LogError($"NovelViewPlaceStateControllerComponent can't find CustomPlaceStatesComponent for {currentPlaceName} prefab");
                }
            }
        }

        private void SetDefaultStateSprite(TimeOfDayEnum timeOfDay, bool setWithFade)
        {
            // Используем словарь для маппинга времени суток на индекс спрайта
            var timeToSpriteIndex = new Dictionary<TimeOfDayEnum, int>
            {
                [TimeOfDayEnum.Morning] = 0,
                [TimeOfDayEnum.Afternoon] = 1,
                [TimeOfDayEnum.Evening] = 2,
                [TimeOfDayEnum.Night] = 3
            };

            if (timeToSpriteIndex.TryGetValue(timeOfDay, out int spriteIndex))
            {
                if (!setWithFade)
                {
                    _spriteRenderer.sprite = defaultStateSprites[spriteIndex];
                }
                else
                {
                    PerformSpriteTransitionUsingShader(defaultStateSprites[spriteIndex]);
                }
            }
            else
            {
                Debug.LogError("There is no valid time of day sprite for the current place");
            }
        }

        //Invokes from event (outside)
        private void UpdatePlaceStateSprite(string placeName, bool setWithFade)
        {
            if (currentPlaceName == placeName)
            {
                SetPlaceStateSprite(setWithFade);
            }
        }
        
        private Sprite GetCurrentPlaceSprite()
        {
            return _spriteRenderer.sprite;
        }
        
        /* //old
        private void SetPlaceStateSprite() 
        {
            var locationManagerInstance = LocationManager.Instance;
            TimeOfDayEnum timeOfDayEnum = locationManagerInstance.GetLocationTimeOfDayState(currentPlaceName);
            string placeState = locationManagerInstance.GetPlaceState(currentPlaceName).ToString();
            Debug.Log(placeState);

            if (placeState == "Default")
            {

                switch (timeOfDayEnum)
                {
                    case TimeOfDayEnum.Morning:
                    {
                        GetComponent<SpriteRenderer>().sprite = defaultStateSprites[0];
                        break;
                    }
                    case TimeOfDayEnum.Afternoon:
                    {
                        GetComponent<SpriteRenderer>().sprite = defaultStateSprites[1];
                        break;
                    }
                    case TimeOfDayEnum.Evening:
                    {
                        GetComponent<SpriteRenderer>().sprite = defaultStateSprites[2];
                        break;
                    }
                    case TimeOfDayEnum.Night:
                    {
                        GetComponent<SpriteRenderer>().sprite = defaultStateSprites[3];
                        break;
                    }
                }
            }
            else
            {
                _customPlaceStatesComponent.GetCustomPlaceStateSprite(timeOfDayEnum, placeState);
            }
        }
        */

        //set old texture to shader (old | alpha =1) -> set new texture to renderer -> reducing alpha from old
        private void PerformSpriteTransitionUsingShader(Sprite newSprite)
        {
            KillCurrentTween();
         
            //EventManager.Instance.InputEvents.SetInputActive(false); //disabling input here breaks skip text functions (lmb\rmb) cause to thread (coroutine) conflicts. Need to use manually (in dialogues etc) 
            
            _oldTexture = _spriteRenderer.sprite.texture; //if we play more than one animation in current place
            _material.SetTexture(OldTex, _oldTexture);
    
            _material.SetFloat(Fade, 1);
            
            // 2. Устанавливаем новую текстуру в спрайт
            _spriteRenderer.sprite = newSprite;
    
            // 4. Устанавливаем смешивание (0 - новая текстура, 1 - старая)
            //_material.SetFloat(Fade, 0.5f); // 50% смешивание

            _tween = _material.DOFloat(0, Fade, _fadeDuration).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    //EventManager.Instance.InputEvents.SetInputActive(true);
                })
                .Play();

        }

        
        /*
        DOTween.To(() => _material.GetFloat("_Fade"), 
        x => _material.SetFloat("_Fade", x), 
        0f, // Конечное значение (полностью новая текстура)
        1f); // Длительность*/
        
       
        
        private bool InAnimation() => _tween != null && _tween.IsActive();

        private void KillCurrentTween()
        {
            if (InAnimation())
            {
                _tween.Kill();
            }
        }
    }
}
