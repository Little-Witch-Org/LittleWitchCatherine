using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.Bench;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class Bench : Location
    {

        public Bench() : base("Bench", TimeOfDayEnum.Morning,
            new List<Place>
            {
                new MainPlaceBench()
            }
        )
        {
            //empty constructor
        }
    }
}