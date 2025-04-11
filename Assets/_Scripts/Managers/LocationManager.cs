using System;
using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.ScriptableObjects.Locations;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private LocationStateSo currentLocationStateSo;
        [SerializeField] private string currenPlace; //todo change to prefab link of place ?

        private void OnEnable()
        {
            EventManager.Instance.TransitionEvents.OnTransitionTriggered += SetTransitionInfo;
            
            EventManager.Instance.TransitionEvents.OnLoadedPlace += UpdateCurrentLocationAndPlace;
            
            EventManager.Instance.TimeEvents.OnTimeOfDayChange += UpdateTimeOfDayForLocations;
            
            
        }

        private void OnDisable()
        {
            EventManager.Instance.TransitionEvents.OnTransitionTriggered -= SetTransitionInfo;
            
            EventManager.Instance.TransitionEvents.OnLoadedPlace -= UpdateCurrentLocationAndPlace;
            
            EventManager.Instance.TimeEvents.OnTimeOfDayChange -= UpdateTimeOfDayForLocations;
        }

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

        private void Initialize()
        {
            currentLocationStateSo = null;
            currenPlace = "";
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

        
        
        public LocationStateSo GetCurrentLocationStateSo()
        {
            return currentLocationStateSo;
        }
        public string GetCurrentPlace()
        {
            return currenPlace;
        }
        
        

        private void UpdateTimeOfDayForLocations(TimeOfDay timeOfDay)
        {
            foreach (var locationSo in locationStates)
            {
                locationSo.TimeOfDay = timeOfDay;
            }
        }

        private void UpdateCurrentLocationAndPlace(string location, string place)
        {
            //Debug.Log("locMan update current location " + location);
            currentLocationStateSo = locationStates.Find(locationState => locationState.name == location);
            
            //Debug.Log("locMan update current place "+ place);
            currenPlace = place;
            
            //invoke method for player and npc
            EventManager.Instance.TransitionEvents.CurrentScreenPlace(location, place);
        }
    }
}