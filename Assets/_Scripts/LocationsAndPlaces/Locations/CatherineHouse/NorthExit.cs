using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.Garden;
using _Scripts.LocationsAndPlaces.Places.NorthExit;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class NorthExit:Location
    {
        public NorthExit() : base("NorthExit", TimeOfDayEnum.Morning,
            new List<Place>
            {
                new MainPlaceNorthExit()
            }
        )
        {
            //empty constructor
        }
    }
}