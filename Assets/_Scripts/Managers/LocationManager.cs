using System;
using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.ScriptableObjects.Locations;
using UnityEngine;

namespace _Scripts.Managers
{
    
    /// <summary>
    /// Manages locations scriptable objects //todo it
    /// Stores location transition info (get from event subscription)
    /// </summary>
    public class LocationManager:MonoBehaviour
    {
        public static LocationManager Instance;
        
        [SerializeField] private string transitionLocation; //uses for transition
        [SerializeField] private string transitionPlace; //uses for transition
        
        [SerializeField] private List<LocationStateSo> locationStates;
        [SerializeField] private LocationStateSo currentLocation;

        private void OnEnable()
        {
            EventManager.Instance.transitionEvents.OnPlaceTransitionTriggered += SetTransitionInfo;
            
            EventManager.Instance.timeEvents.OnTimeOfDayChange += UpdateTimeOfDayForLocations;
            
            
        }

        private void OnDisable()
        {
            EventManager.Instance.transitionEvents.OnPlaceTransitionTriggered -= SetTransitionInfo;
            
            EventManager.Instance.timeEvents.OnTimeOfDayChange -= UpdateTimeOfDayForLocations;
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
            
            currentLocation = locationStates.Find(locationState => locationState.name == location);
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
    }
}