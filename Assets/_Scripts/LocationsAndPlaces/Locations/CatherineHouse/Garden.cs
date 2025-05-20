using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.Bench;
using _Scripts.LocationsAndPlaces.Places.Garden;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class Garden:Location
    {
        public Garden() : base("Garden", TimeOfDayEnum.Morning,
            new List<Place>
            {
                new MainPlaceGarden()
            }
        )
        {
            //empty constructor
        }
    }
}