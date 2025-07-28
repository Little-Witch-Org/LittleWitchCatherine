using System;
using UnityEngine;

namespace _Scripts.LocationsAndPlaces.Places.CatherineHouse
{
    public class GuestBedroom:Place
    {
        public new enum PlaceStateEnum
        {
            Default,
            Astral
        }
        public GuestBedroom()
        {
            PlaceName = "GuestBedroom";
            IsLocked =  false;
        }
        
        public new PlaceStateEnum CurrentPlaceState //synchronized with base Place state
        {
            get => (PlaceStateEnum)base.CurrentPlaceState;
            set => base.CurrentPlaceState = (Place.PlaceStateEnum)value;
        }
        
        public override void SetPlaceState(Enum placeState)
        {
            if (placeState is PlaceStateEnum state)
            {
                CurrentPlaceState = state;
            }
            else
            {
                Debug.LogError($"Invalid state type for {PlaceName}. Expected {typeof(PlaceStateEnum)}, got {placeState.GetType()}");
            }
        }
        
        public override Enum GetPlaceState( )
        {
            return CurrentPlaceState;
        }
    }
}