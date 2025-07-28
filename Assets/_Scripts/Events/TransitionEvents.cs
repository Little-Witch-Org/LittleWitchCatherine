using System;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Maps;
using UnityEngine;

namespace _Scripts.Events
{
    /// <summary>
    /// Place transition: OnPlaceTransitionTrigger -> OnPlaceTransitionPerform -> OnLoadedPlace -> OnCurrentPlaceOnScreen.
    /// </summary>
    public class TransitionEvents
    {
        private bool _isTransitionDisabled;

        public void SetTransitionDisabled(bool isDisabled)
        {
            _isTransitionDisabled = isDisabled;
        }

        public bool GetIsTransitionDisabled()
        {
            return _isTransitionDisabled;
        }
        
        
        //set location and place for transition in transition manager. NovelViewChangePlaceAfterTransitionComponent gets which was set info from transition manager 
        public event Action<string,string> OnPlaceTransitionTrigger;
        public void PlaceTransitionTrigger(string location, string place) 
        {
            OnPlaceTransitionTrigger?.Invoke(location, place);
        }
        
        //Invokes by Transition Manager. NovelViewChangePlaceAfterTransitionComponent listens and perform place transition.
        public event Action OnPlaceTransitionPerform;
        public void PlaceTransitionPerform() 
        {
            OnPlaceTransitionPerform?.Invoke();
        }
        
        
        //uses to update current location and place field in Transition Manager after transition.
        public event Action<string, string> OnLoadedPlace;
        public void LoadedPlace(string location, string place) 
        {
            OnLoadedPlace?.Invoke(location, place);
        }
        
        //used to update current location and place in Player Char Manager after place loaded and "current" field in manager set.
        //Npc manager uses this event to check condition for npc appear/disappear (need to do it instantly after change view cause we need to spawn npc before fade out.
        public event Action<string, string> OnCurrentPlaceOnScreen;
        public void CurrentPlaceOnScreen(string location, string place) 
        {
            OnCurrentPlaceOnScreen?.Invoke(location, place);
        }
        
        public event Action<SceneNamesEnum> OnChangeScene;
        public void ChangeScene(SceneNamesEnum scene) 
        {
            OnChangeScene?.Invoke(scene);
        }
        
        public event Action<Map> OnLoadedMap;
        public void LoadedMap(Map map) 
        {
            OnLoadedMap?.Invoke(map);
        }
        
        public event Action<string,string> OnTeleportPlayerBetweenPlaces;
        public void TeleportPlayerBetweenPlaces(string location, string place) 
        {
            OnTeleportPlayerBetweenPlaces?.Invoke(location, place);
        }
        
    }
}