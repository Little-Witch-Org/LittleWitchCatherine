using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse;
using _Scripts.LocationsAndPlaces.Places.Greenhouse;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class Greenhouse:Location
    {
        public Greenhouse() : base("Greenhouse", TimeOfDayEnum.Morning,
            new List<Place>
            {
                new MainPlaceGreenhouse()
            }
        )
        {
            //empty constructor
        }
    }
}