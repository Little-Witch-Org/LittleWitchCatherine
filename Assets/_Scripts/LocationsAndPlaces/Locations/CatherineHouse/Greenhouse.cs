using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse.Seesaw;
using _Scripts.LocationsAndPlaces.Places.Greenhouse;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class Greenhouse:Location
    {
        public Greenhouse() : base("Greenhouse", TimeOfDay.Morning,
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