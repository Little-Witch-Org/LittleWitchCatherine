using System;
using UnityEngine;

namespace _Scripts.Events
{
    public class TransitionEvents
    {
        public event Action<string,string> OnPlaceTransitionTriggered;
        public void TriggerTransition(string location, string place) 
        {
            OnPlaceTransitionTriggered?.Invoke(location, place);
        }
        
        
        //uses to update current place field in Location Manager and Character Manager after transition (Place prefab Instantiation)
        public event Action<string> OnSetLoadedPlaceName;
        public void SetLoadedPlaceName( string place) 
        {
            OnSetLoadedPlaceName?.Invoke(place);
        }
        
        //uses to update current location field in Location Manager and Character Manager after transition (Place prefab Instantiation)
        public event Action<string> OnSetLoadedLocationName;
        public void SetLoadedLocationName( string location) 
        {
            OnSetLoadedLocationName?.Invoke(location);
        }
    }
}