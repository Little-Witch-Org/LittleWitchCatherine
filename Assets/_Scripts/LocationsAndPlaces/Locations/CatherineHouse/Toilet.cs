using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.Garden;
using _Scripts.LocationsAndPlaces.Places.Toilet;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class Toilet:Location
    {
        public Toilet() : base("Toilet", TimeOfDayEnum.Morning,
            new List<Place>
            {
                new MainPlaceToilet()
            }
        )
        {
            //empty constructor
        }
    }
}