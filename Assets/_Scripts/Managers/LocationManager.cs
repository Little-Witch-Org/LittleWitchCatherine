using System;
using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.ScriptableObjects.Locations;
using UnityEngine;

namespace _Scripts.Managers
{
    
    /// <summary>
    /// Manages locations scriptable objects //todo it
    /// Stores location transition Info (get from event subscription) //todo need to handle exit on map (clear fields)
    /// </summary>
    public class LocationManager:MonoBehaviour
    {
        public static LocationManager Instance;
        
        [SerializeField] private string transitionLocation; //uses for transition
        [SerializeField] private string transitionPlace; //uses for transition
        
        [SerializeField] private List<LocationStateSo> locationStates;
        [SerializeField] private LocationStateSo currentLocation;
        [SerializeField] private string currenPlace; //todo change to prefab link ?

        private void OnEnable()
        {
            EventManager.Instance.TransitionEvents.OnPlaceTransitionTriggered += SetTransitionInfo;
            
            EventManager.Instance.TransitionEvents.OnSetLoadedPlaceName += UpdateCurrentPlace;
            EventManager.Instance.TransitionEvents.OnSetLoadedLocationName += UpdateCurrentLocation;
            
            EventManager.Instance.TimeEvents.OnTimeOfDayChange += UpdateTimeOfDayForLocations;
            
            
        }

        private void OnDisable()
        {
            EventManager.Instance.TransitionEvents.OnPlaceTransitionTriggered -= SetTransitionInfo;
            
            EventManager.Instance.TransitionEvents.OnSetLoadedPlaceName -= UpdateCurrentPlace;
            EventManager.Instance.TransitionEvents.OnSetLoadedLocationName -= UpdateCurrentLocation;
            
            EventManager.Instance.TimeEvents.OnTimeOfDayChange -= UpdateTimeOfDayForLocations;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
            
            
            //todo add default variables initialization depending on start scene
        }

        
        private void SetTransitionInfo(string location, string place)
        {
            transitionLocation = location;
            transitionPlace = place;
            
            /*Debug.Log(location);
            Debug.Log(place);*/
            
            
        }

        public string GetTransitionLocation()
        {
            return transitionLocation;
        }

        public string GetTransitionPlace()
        {
            return transitionPlace;
        }

        public LocationStateSo GetCurrentLocation()
        {
            return currentLocation;
        }
        

        private void UpdateTimeOfDayForLocations(TimeOfDay timeOfDay)
        {
            foreach (var locationSo in locationStates)
            {
                locationSo.TimeOfDay = timeOfDay;
            }
        }

        private void UpdateCurrentPlace(string place)
        {
            currenPlace = place;
        }
        private void UpdateCurrentLocation(string location)
        {
            currentLocation = locationStates.Find(locationState => locationState.name == location);
        }
    }
}