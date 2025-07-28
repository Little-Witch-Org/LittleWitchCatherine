using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Components.Transition
{
    /// <summary>
    /// Listen trigger invocation and get transition info from Transition manager and instantiate Place prefab
    /// Invoke LoadedPlace after prefab loaded (uses by Location manager)
    /// </summary>
    public class NovelViewChangePlaceAfterTransitionComponent : MonoBehaviour
    {

        [SerializeField] private BoxCollider2D clickBlocker;
        [SerializeField] private SpriteRenderer fadeImageRenderer;

        private float _placeFadeDuration;
        
        private Sequence _sequence;
        private GameObject _currentRoomInstance;

        private bool _isInCutscene;
        
        private void Start()
        {
            if (TransitionManager.Instance == null)
            {
                Debug.LogError("NovelViewChangePlaceAfterTransitionComponent can't find transition manager");
                return;
            }

            ChangeView(); //use on start after scene transition (gets saved info from transition manager (on map)
        }

        public void ChangeView()
        {
            //get fade duration
            _placeFadeDuration= TransitionManager.Instance.GetPlaceFadeDuration();
            
            //Delete place game object if it exists
            if (_currentRoomInstance != null)
            {
                Destroy(_currentRoomInstance);
            }
            
            string mapName = "CatherineHouseMap";
            string locationName = TransitionManager.Instance.GetTransitionLocation();
            string placeName = TransitionManager.Instance.GetTransitionPlace();

            //Debug.Log($"Prefabs/NovelViewPlaces/{mapName}/{locationName}/{placeName}");
            
            string prefabPath = $"Prefabs/NovelViewPlaces/{mapName}/{locationName}/{placeName}";
            
            GameObject roomPrefab = UnityEngine.Resources.Load(prefabPath) as GameObject;
            
            
            if (roomPrefab != null)
            {
                _currentRoomInstance = Instantiate(roomPrefab);

                //Debug.Log("Loaded new room successfully");
                //Debug.Log("invoke loaded location and place " + locationName + " " + placeName);
                EventManager.Instance.TransitionEvents.LoadedPlace(locationName, placeName);
            }
            else
            {
                Debug.LogError($"Prefab '{prefabPath}' not found.");
            }
            
        }

        private void OnEnable()
        {
            EventManager.Instance.TransitionEvents.OnPlaceTransitionPerform += ChangePlaceWithFade;
            EventManager.Instance.CutsceneEvents.OnCutsceneStarted+= OnCutsceneStarted;
            EventManager.Instance.CutsceneEvents.OnCutsceneFinished+= OnCutsceneFinished;
        }

        private void OnDisable()
        {
            EventManager.Instance.TransitionEvents.OnPlaceTransitionPerform -= ChangePlaceWithFade;
        }

        private void OnDestroy()
        {
            KillCurrentSequence();
        }


        private void ChangePlaceWithFade()
        {
            //Debug.Log("Change place with fade");
            //disable hotkeys (menu)
            if (!_isInCutscene)
            {
                EventManager.Instance.InputEvents.SetHotkeysActive(false);
            }

            _sequence = DOTween.Sequence();

            //activate box collider to prevent clicks on other colliders
            _sequence.AppendCallback(() => { clickBlocker.enabled = true; });

            //fadein 
            _sequence.Append(fadeImageRenderer.DOFade(1f, _placeFadeDuration).SetEase(Ease.OutCubic));

            //Change view
            _sequence.AppendCallback(ChangeView);

            //fadeout
            _sequence.Append(fadeImageRenderer.DOFade(0f, _placeFadeDuration).SetEase(Ease.InCubic));

            //disable box collider to prevent clicks on other colliders
            _sequence.AppendCallback(() => { clickBlocker.enabled = false; });

            _sequence.Play().OnComplete(() =>
            {
                //enable hotkeys (menu)
                if (!_isInCutscene)
                {
                    EventManager.Instance.InputEvents.SetHotkeysActive(true);
                }
            });
            
        }


        private bool InAnimation() => _sequence != null && _sequence.IsActive();

        private void KillCurrentSequence()
        {
            if (InAnimation())
            {
                _sequence.Kill();
            }
        }

        private void OnCutsceneStarted()
        {
            _isInCutscene = true;
        }

        private void OnCutsceneFinished()
        {
            _isInCutscene = false;
        }
    }

}
