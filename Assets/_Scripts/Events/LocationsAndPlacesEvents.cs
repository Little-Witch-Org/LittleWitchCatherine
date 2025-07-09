using System;
using UnityEngine;

namespace _Scripts.Events
{
    public class LocationsAndPlacesEvents
    {
        //used to signal novel camera that place image changed and need to adjust camera size
        public event Action<SpriteRenderer> OnPlaceSpriteChanged;
        public void PlaceSpriteChanged(SpriteRenderer spriteRenderer) 
        {
            OnPlaceSpriteChanged?.Invoke(spriteRenderer);
        }
        
        public event Action<string,bool> OnSetAllPlacesLockState;
        public void SetAllPlacesLockState(string location, bool isLocked) 
        {
            OnSetAllPlacesLockState?.Invoke(location, isLocked);
        }

        public event Action<string, bool, string[]> OnSetPlacesLockState;

        public void SetPlacesLockState(string location, bool state, params string[] places)
        {
            OnSetPlacesLockState?.Invoke(location, state, places);
        }
        
        public event Action<bool> OnSetAllLocationsLockState;
        public void SetAllLocationsLockState( bool isLocked) 
        {
            OnSetAllLocationsLockState?.Invoke(isLocked);
        }

        public event Action<string, bool> OnSetLocationLockState;

        public void SetLocationLockState(string location, bool state)
        {
            OnSetLocationLockState?.Invoke(location, state);
        }
        
        public event Action<string, Enum> OnSetPlaceState;

        public void SetPlaceState(string placeName, Enum placeState)
        {
            OnSetPlaceState?.Invoke(placeName, placeState);
        }
        
        public event Func<string, Enum> OnGetPlaceState;

        public Enum GetPlaceState(string placeName)
        {
            return OnGetPlaceState?.Invoke(placeName);
        } 
        
        public event Func<Sprite> OnGetCurrentPlaceSprite;
        public Sprite GetCurrentPlaceSprite()
        {
            return OnGetCurrentPlaceSprite?.Invoke();
        } 
        
        public event Action<string, Enum> OnSetPlaceStateAndApply;

        public void SetPlaceStateAndApply(string placeName, Enum placeState)
        {
            OnSetPlaceStateAndApply?.Invoke(placeName, placeState);
        }
        
        public event Action<string> OnUpdatePlaceStateSprite;

        public void UpdatePlaceStateSprite(string placeName)
        {
            OnUpdatePlaceStateSprite?.Invoke(placeName);
        }

    }
}