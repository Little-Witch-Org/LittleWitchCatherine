using System;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace _Scripts.Components.TransitionComponents
{
    /// <summary>
    /// Listen trigger invocation and get transition info from Transition manager and instantiate Place prefab
    /// Invoke LoadedPlace after prefab loaded (uses by Location manager)
    /// </summary>
    public class NovelViewChangePlaceAfterTransitionComponent : MonoBehaviour
    {

        [SerializeField] private BoxCollider2D clickBlocker;
        [SerializeField] private SpriteRenderer fadeImageRenderer;

        private Sequence _sequence;
        private GameObject currentRoomInstance;

        public void ChangeView()
        {
            //Delete place game object if it exists
            if (currentRoomInstance != null)
            {
                Destroy(currentRoomInstance);
            }
            
            string mapName = "CatherineHouseMap";
            string locationName = TransitionManager.Instance.GetTransitionLocation();
            string placeName = TransitionManager.Instance.GetTransitionPlace();

            //Debug.Log($"Prefabs/NovelViewPlaces/{mapName}/{locationName}/{placeName}");
            
            string prefabPath = $"Prefabs/NovelViewPlaces/{mapName}/{locationName}/{placeName}";
            
            GameObject roomPrefab = UnityEngine.Resources.Load(prefabPath) as GameObject;
            
            
            if (roomPrefab != null)
            {
                currentRoomInstance = Instantiate(roomPrefab);

                //Debug.Log("Loaded new room successfully");
                //Debug.Log("invoke loaded location and place " + locationName + " " + placeName);
                EventManager.Instance.TransitionEvents.LoadedPlace(locationName, placeName);
            }
            else
            {
                Debug.LogError($"Prefab '{prefabPath}' not found.");
            }
            
        }

        private void Start()
        {
            if (TransitionManager.Instance == null)
            {
                Debug.LogError("NovelViewChangePlaceAfterTransitionComponent can't find transition manager");
                return;
            }

            ChangeView(); //use on start after scene transition
        }

        private void OnEnable()
        {
            EventManager.Instance.TransitionEvents.OnPlaceTransitionPerform += ChangePlaceWithFade;
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
            //disable hotkeys (menu)
            EventManager.Instance.InputEvents.HotkeysAreActive(false);

            _sequence = DOTween.Sequence();

            //activate box collider to prevent clicks on other colliders
            _sequence.AppendCallback(() => { clickBlocker.enabled = true; });

            //fadein 
            _sequence.Append(fadeImageRenderer.DOFade(1f, 0.5f).SetEase(Ease.OutCubic));

            //Change view
            _sequence.AppendCallback(ChangeView);

            //fadeout
            _sequence.Append(fadeImageRenderer.DOFade(0f, 0.5f).SetEase(Ease.InCubic));

            //disable box collider to prevent clicks on other colliders
            _sequence.AppendCallback(() => { clickBlocker.enabled = false; });

            _sequence.Play().OnComplete(() =>
            {
                //enable hotkeys (menu)
                EventManager.Instance.InputEvents.HotkeysAreActive(true);
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
    }

}
