using System;

namespace _Scripts.Events
{
    public class LocationsAndPlacesEvents
    {
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

    }
}