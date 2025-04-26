using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse.Seesaw;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class Seesaw : Location
    {

        public Seesaw() : base("Seesaw", TimeOfDay.Morning,
            new List<Place>
            {
                new MainPlaceSeesaw()
            }
        )
        {
            //empty constructor
        }
    }

}