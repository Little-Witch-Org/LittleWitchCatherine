using System;
using UnityEngine;

namespace _Scripts.LocationsAndPlaces.Places
{
    /// <summary>
    /// Stores info about place (with custom states)
    /// </summary>
    public abstract class Place
    {

        public enum PlaceStateEnum
        {
            Default
        }

        public Place()
        {
            CurrentPlaceState = PlaceStateEnum.Default;
        }

        public string PlaceName { get; protected set; }
        public bool IsLocked { get; set; }
        
        public PlaceStateEnum CurrentPlaceState { get; set; }
        
        public virtual void SetPlaceState(Enum state)
        {
            if (state is PlaceStateEnum placeState)
            {
                CurrentPlaceState = placeState;
            }
            else
            {
                Debug.LogError($"Invalid state type for {PlaceName}. Expected {typeof(PlaceStateEnum)}, got {state.GetType()}");
            }
        }
    
        public virtual Enum GetPlaceState()
        {
            return CurrentPlaceState;
        }
    }
}