using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Components.Transition;
using _Scripts.Components.Transition.UI;
using _Scripts.Enums;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Managers
{
    
    /// <summary>
    /// Stores location transition Info (get from event subscription) //todo need to handle exit on map (clear fields)
    /// Load scenes after trigger (enter/exit)
    /// </summary>
    public class TransitionManager:MonoBehaviour
    {
        public static TransitionManager Instance;
        
        private SceneLoader _sceneLoader;
        
        [SerializeField] private string transitionLocation; //uses for transition
        [SerializeField] private string transitionPlace; //uses for transition
        
        
        [SerializeField] private string currentLocation;
        [SerializeField] private string currenPlace; //todo change to prefab link of place ?
        
        
        [SerializeField] private float placeFadeDuration=0.5f;
        

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Initialize();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }
        
        
        private void OnEnable()
        {
            EventManager.Instance.TransitionEvents.OnPlaceTransitionTrigger += SetPlaceTransitionInfo;
            
            EventManager.Instance.TransitionEvents.OnLoadedPlace += UpdateCurrentLocationAndPlace;
            
            EventManager.Instance.TransitionEvents.OnChangeScene += ChangeScene;
            
            EventManager.Instance.TransitionEvents.OnTeleportPlayerBetweenPlaces += TeleportPlayerBetweenPlaces;
            
        }

        private void OnDisable()
        {
            EventManager.Instance.TransitionEvents.OnPlaceTransitionTrigger -= SetPlaceTransitionInfo;
            
            EventManager.Instance.TransitionEvents.OnLoadedPlace -= UpdateCurrentLocationAndPlace;
            
            EventManager.Instance.TransitionEvents.OnChangeScene -= ChangeScene;
            
            EventManager.Instance.TransitionEvents.OnTeleportPlayerBetweenPlaces -= TeleportPlayerBetweenPlaces;
        }
        
        private void Initialize()
        {
            currentLocation = "";
            currenPlace = "";
            _sceneLoader = new SceneLoader();
        }

        private void ChangeScene(SceneNamesEnum scene)
        {
            StartCoroutine(ChangeSceneCoroutine(scene));
        }
        
        private IEnumerator ChangeSceneCoroutine(SceneNamesEnum scene) //todo disable input while loading
        {
            //disable hotkeys (menu)
            EventManager.Instance.InputEvents.SetHotkeysActive(false);
            
            //fade in screen
            UIManager.Instance.loadScreenUI.FadeInLoadingScreen();
            yield return new WaitForSeconds(0.7f);
            
            //add loading animation
            UIManager.Instance.loadScreenUI.LoadingProgressAnimation();
            
            //load scene
            yield return _sceneLoader.LoadSceneAsync(scene);
            
            yield return new WaitForSeconds(0.3f);
            
            //stop loading animation
            UIManager.Instance.loadScreenUI.FadeOutLoadingScreen();
            
            //enable hotkeys (menu)
            EventManager.Instance.InputEvents.SetHotkeysActive(true);
            
        }
        
        
        private void SetPlaceTransitionInfo(string location, string place)
        {
            transitionLocation = location;
            transitionPlace = place;
            
            /*Debug.Log(location);
            Debug.Log(place);*/
            
            //Invoke place transition. NovelViewChangePlaceAfterTransitionComponent listens and process place transition
            EventManager.Instance.TransitionEvents.PlaceTransitionPerform();
        }

        public string GetTransitionLocation()
        {
            return transitionLocation;
        }

        public string GetTransitionPlace()
        {
            return transitionPlace;
        }

        
        
        public string GetCurrentLocation()
        {
            return currentLocation;
        }
        public string GetCurrentPlace()
        {
            return currenPlace;
        }
        

        private void UpdateCurrentLocationAndPlace(string location, string place)
        {
            //Debug.Log("locMan update current location " + location);
            currentLocation = location;
            
            //Debug.Log("locMan update current place "+ place);
            currenPlace = place;
            
            //invoke method for player and npc
            EventManager.Instance.TransitionEvents.CurrentPlaceOnScreen(location, place);
        }

        private void TeleportPlayerBetweenPlaces(string location, string place)
        {
            EventManager.Instance.TransitionEvents.PlaceTransitionTrigger(location, place);
        }

        public float GetPlaceFadeDuration()
        {
            return placeFadeDuration;
        }
    }
}