using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.Bench;
using _Scripts.LocationsAndPlaces.Places.Flashlight;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class Flashlight:Location
    {
        public Flashlight() : base("Flashlight", TimeOfDayEnum.Morning,
            new List<Place>
            {
                new MainPlaceFlashlight()
            }
        )
        {
            //empty constructor
        }
    }
}