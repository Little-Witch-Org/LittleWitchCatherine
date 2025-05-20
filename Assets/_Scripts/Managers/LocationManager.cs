using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces;
using _Scripts.LocationsAndPlaces.Locations;
using _Scripts.LocationsAndPlaces.Locations.CatherineHouse;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse;
using UnityEngine;
using UnityEngine.Serialization;


namespace _Scripts.Managers
{
    
    /// <summary>
    /// Stores locations and places. Manage their conditions.
    /// </summary>
    public class LocationManager : MonoBehaviour
    {   
        public static LocationManager Instance;
        
        //[SerializeField] private List<LocationStateSo> locationStates;
        //public LocationStateSo currentLocationState;

        private Dictionary<string, Location> _locationsDictionary;
        //private string _currentLocation;
        
        [SerializeField] private TimeOfDayEnum currentTimeOfDayEnum; //only for inspector
        
        private void Awake()
        {
            if (Instance == null)
            {
                LocationsInitialization();
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /*private void Start()
        {
            foreach (var pair in _locationsDictionary)
            {
               Debug.Log(pair.Value.Places.Count);
               foreach (var place in pair.Value.Places)
               {
                   Debug.Log(place.PlaceName);
                   Debug.Log(place.IsLocked);
               }
            }
        }*/

        private void LocationsInitialization()
        {
            _locationsDictionary = new Dictionary<string, Location>();
            _locationsDictionary["CatherineHouse"] = new CatherineHouse();
            _locationsDictionary["Seesaw"] = new Seesaw();
            _locationsDictionary["Greenhouse"] = new Greenhouse();
            _locationsDictionary["Bench"] = new Bench();
            _locationsDictionary["Flashlight"] = new Flashlight();
            _locationsDictionary["Garden"] = new Garden();
            _locationsDictionary["NorthExit"] = new NorthExit();
            _locationsDictionary["Pit"] = new Pit();
            _locationsDictionary["SouthExit"] = new SouthExit();
            _locationsDictionary["Toilet"] = new Toilet();
            _locationsDictionary["Bathhouse"] = new Bathhouse();
            
            //Debug.Log(_locationsDictionary["CatherineHouse"].Places.Count);
        }
        
        private void OnApplicationQuit()
        {
            Destroy(Instance);
        }
        

        private void OnEnable()
        {
            EventManager.Instance.TimeEvents.OnUpdateTimeOfDay += UpdateTimeOfDayForLocations;
            //EventManager.Instance.TransitionEvents.OnLoadedPlace += SetCurrentLocation;

            EventManager.Instance.LocationsAndPlacesEvents.OnSetAllPlacesLockState += SetAllPlacesLockState;
            EventManager.Instance.LocationsAndPlacesEvents.OnSetPlacesLockState += SetPlacesLockState;
            EventManager.Instance.LocationsAndPlacesEvents.OnSetAllLocationsLockState += SetAllLocationsLockState;
            EventManager.Instance.LocationsAndPlacesEvents.OnSetLocationLockState += SetLocationLockState;
            
        }

        private void OnDisable()
        {
            EventManager.Instance.TimeEvents.OnUpdateTimeOfDay -= UpdateTimeOfDayForLocations;
            //EventManager.Instance.TransitionEvents.OnLoadedPlace -= SetCurrentLocation;
            
            EventManager.Instance.LocationsAndPlacesEvents.OnSetAllPlacesLockState -= SetAllPlacesLockState;
            EventManager.Instance.LocationsAndPlacesEvents.OnSetPlacesLockState -= SetPlacesLockState;
            EventManager.Instance.LocationsAndPlacesEvents.OnSetAllLocationsLockState -= SetAllLocationsLockState;
            EventManager.Instance.LocationsAndPlacesEvents.OnSetLocationLockState -= SetLocationLockState;
        }

        /*public string GetCurrentLocation()
        {
            return _currentLocation;
        }

        private void SetCurrentLocation(string location, string place)
        {
            _currentLocation = location;
        }*/

        public bool IsPlaceLocked(string placeName)
        {
            return FindPlaceInAnyLocation(placeName).IsLocked;
        }

        public bool IsLocationLocked(string locationName)
        {
            if (_locationsDictionary.TryGetValue(locationName, out Location location))
            {
                return location.isLocationLocked;
            }
           
            throw new ArgumentException($"Location '{locationName}' not found in dictionary!");
        }

        private Place FindPlaceInAnyLocation(string placeName) =>
            _locationsDictionary.Values
                .SelectMany(loc => loc.Places)
                .FirstOrDefault(place => place.PlaceName == placeName);
        
        public TimeOfDayEnum GetLocationTimeOfDayState(string placeName)
        { 
            
            foreach (var locationPair in _locationsDictionary)
            {
                var location = locationPair.Value;
                
                var foundPlace = location.Places.Find(p => p.PlaceName == placeName);
                if (foundPlace != null)
                {
                    return location.TimeOfDayEnum; 
                }
            }
    
            throw new ArgumentException($"Place '{placeName}' was not found in any location!");
        }

        private void UpdateTimeOfDayForLocations(TimeOfDayEnum timeOfDayEnum)
        {
            currentTimeOfDayEnum = timeOfDayEnum;
            
            foreach (var pair in _locationsDictionary)
            {
                pair.Value.TimeOfDayEnum = timeOfDayEnum;
            }
        }

        private void SetAllPlacesLockState(string location, bool isLocked)
        {
            if (!_locationsDictionary.TryGetValue(location, out var locationObj))
            {
                Debug.LogError($"Location '{location}' not found!");
                return;
            }

            foreach (var place in locationObj.Places)
            {
                place.IsLocked = isLocked;
            }
        }

        private void SetPlacesLockState(string location, bool isLocked, params string[] places)
        {
            if (places == null || !_locationsDictionary.TryGetValue(location, out var locationObj))
            {
                Debug.LogError($"Location '{location}' not found! or the places list is empty!");
                return;
            }

            foreach (var place in places)
            {
                var foundPlace = locationObj.Places.Find(x => x.PlaceName == place);
                if (foundPlace != null)
                    foundPlace.IsLocked = isLocked;
            }
        }
        
        private void SetAllLocationsLockState(bool isLocked)
        {
            foreach (var pair in _locationsDictionary)
            {
                pair.Value.isLocationLocked = isLocked;
                Debug.Log($"Location '{pair.Key}' is locked: {isLocked}");
            }
        }
        
        private void SetLocationLockState(string locationName, bool isLocked)
        {
            if (_locationsDictionary.TryGetValue(locationName, out Location location))
            {
                location.isLocationLocked = isLocked;
            }
            else
            {
                Debug.LogWarning($"Location '{locationName}' not found in dictionary!");
            }
        }
        
    }
}