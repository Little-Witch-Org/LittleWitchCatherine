using System;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Components.Transition
{
    /// <summary>
    /// Component stores default sprites of novel Places.
    /// Changes sprite depending on requested state from Place class using Location Manager.
    /// Handle custom states (sprites) by using additional component on prefab (CustomPlaceStatesComponent)
    /// </summary>
    public class NovelViewPlaceStateControllerComponent : MonoBehaviour
    {
        [SerializeField] private string currentPlaceName;
        
        private CustomPlaceStatesComponent _customPlaceStatesComponent;
        
        [FormerlySerializedAs("placeStateSprites")] [FormerlySerializedAs("roomStateSprites")] [SerializeField] private List<Sprite> defaultStateSprites;
        
        private SpriteRenderer _spriteRenderer;


        private void OnEnable()
        {
            EventManager.Instance.LocationsAndPlacesEvents.OnUpdatePlaceStateSprite += UpdatePlaceStateSprite;
            EventManager.Instance.LocationsAndPlacesEvents.OnGetCurrentPlaceSprite += GetCurrentPlaceSprite;
        }

        private void OnDisable()
        {
            EventManager.Instance.LocationsAndPlacesEvents.OnUpdatePlaceStateSprite -= UpdatePlaceStateSprite;
            EventManager.Instance.LocationsAndPlacesEvents.OnGetCurrentPlaceSprite -= GetCurrentPlaceSprite;
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
            
            //get appropriate sprite for place on start
            SetPlaceStateSprite();
            EventManager.Instance.LocationsAndPlacesEvents.PlaceSpriteChanged(_spriteRenderer);
        }
        
        private void SetPlaceStateSprite()
        {
            var locationManager = LocationManager.Instance;
            TimeOfDayEnum timeOfDay = locationManager.GetLocationTimeOfDayState(currentPlaceName);
            string placeState = locationManager.GetPlaceState(currentPlaceName).ToString();
    
            if (placeState == "Default") //place enum = default
            {
                SetDefaultStateSprite(timeOfDay);
            }
            else
            {
                if (_customPlaceStatesComponent != null)
                {
                    _customPlaceStatesComponent.SetCustomPlaceStateSprite(timeOfDay, placeState);
                }
                else
                {
                    Debug.LogError($"NovelViewPlaceStateControllerComponent can't find CustomPlaceStatesComponent for {currentPlaceName} prefab");
                }
            }
        }

        private void SetDefaultStateSprite(TimeOfDayEnum timeOfDay)
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
                GetComponent<SpriteRenderer>().sprite = defaultStateSprites[spriteIndex];
            }
        }

        private void UpdatePlaceStateSprite(string placeName)
        {
            if (currentPlaceName == placeName)
            {
                SetPlaceStateSprite();
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
                _customPlaceStatesComponent.SetCustomPlaceStateSprite(timeOfDayEnum, placeState);
            }
        }
        */
        
        
        
    }
}
