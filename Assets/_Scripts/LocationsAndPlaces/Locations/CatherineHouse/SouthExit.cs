using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.SouthExit;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class SouthExit:Location
    {
        public SouthExit() : base("SouthExit", TimeOfDayEnum.Morning,
            new List<Place>
            {
                new MainPlaceSouthExit()
            }
        )
        {
            //empty constructor
        }
    }
}