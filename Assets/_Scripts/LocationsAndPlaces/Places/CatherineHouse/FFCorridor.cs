using System;
using _Scripts.Enums;
using UnityEngine;

namespace _Scripts.LocationsAndPlaces.Places.CatherineHouse
{
    public class FFCorridor:Place
    {
        public new enum PlaceStateEnum // add "new" to cover base enum
        {
            Default,
            Custom1,
            Custom2,
            BrokenDoor
        }
        
        public FFCorridor()
        {
            PlaceName = "FFCorridor";
            IsLocked =  false;
            //current = default from Place
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