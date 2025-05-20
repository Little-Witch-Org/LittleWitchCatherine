using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.Garden;
using _Scripts.LocationsAndPlaces.Places.Pit;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class Pit:Location
    {
        public Pit() : base("Pit", TimeOfDayEnum.Afternoon,
            new List<Place>
            {
                new MainPlacePit()
            }
        )
        {
            //empty constructor
        }
    }
}