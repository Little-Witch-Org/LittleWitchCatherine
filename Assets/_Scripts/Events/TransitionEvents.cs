using System;
using UnityEngine;

namespace _Scripts.Events
{
    public class TransitionEvents
    {
        //set location and place for transition. Used in location manager. NovelViewChangePlaceAfterTransitionComponent gets this info from location manager 
        public event Action<string,string> OnTransitionTriggered;
        public void TransitionTriggered(string location, string place) 
        {
            OnTransitionTriggered?.Invoke(location, place);
        }
        
        
        //uses to update current location and place field in Location Manager after transition (Place prefab Instantiation)
        public event Action<string, string> OnLoadedPlace;
        public void LoadedPlace(string location, string place) 
        {
            OnLoadedPlace?.Invoke(location, place);
        }
        
        /*//uses to update current location field in Location Manager and Character Manager after transition (Place prefab Instantiation)
        public event Action<string> OnSetLoadedLocationName;
        public void SetLoadedLocationName( string location) 
        {
            OnSetLoadedLocationName?.Invoke(location);
        }*/
        
        //used to update current location and place in Player Char Manager after place loaded and "current" field in manager set. Npc manager uses this event to check condition for npc appear/disappear
        public event Action<string, string> OnCurrentScreenPlace;
        public void CurrentScreenPlace(string location, string place) 
        {
            OnCurrentScreenPlace?.Invoke(location, place);
        }
    }
}