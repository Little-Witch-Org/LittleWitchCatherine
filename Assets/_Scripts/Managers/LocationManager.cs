using System;
using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.ScriptableObjects.Locations;
using UnityEngine;


namespace _Scripts.Managers
{
    
    /// <summary>
    /// Manages locations scriptable objects //todo change So for default class ? (so saves info in runtime)
    /// todo dont like set current location and then set time of day to places. to remake.
    /// </summary>
    public class LocationManager : MonoBehaviour
    {   
        public static LocationManager Instance;
        
        [SerializeField] private List<LocationStateSo> locationStates;
        
        public LocationStateSo currentLocationState;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void OnApplicationQuit()
        {
            GameObject.Destroy(Instance);
        }
        

        private void OnEnable()
        {
            EventManager.Instance.TimeEvents.OnTimeOfDayChange += UpdateTimeOfDayForLocations;
            EventManager.Instance.TransitionEvents.OnLoadedPlace += SetCurrentLocationState;
        }

        private void OnDisable()
        {
            EventManager.Instance.TimeEvents.OnTimeOfDayChange -= UpdateTimeOfDayForLocations;
            EventManager.Instance.TransitionEvents.OnLoadedPlace -= SetCurrentLocationState;
        }

        public LocationStateSo GetCurrentLocationState()
        {
            return currentLocationState;
        }

        private void SetCurrentLocationState(string location, string place)
        {
            currentLocationState = locationStates.Find(x => x.LocationName == location);
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