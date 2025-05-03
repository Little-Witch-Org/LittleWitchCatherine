using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse;
using _Scripts.LocationsAndPlaces.Places.Seesaw;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class Seesaw : Location
    {

        public Seesaw() : base("Seesaw", TimeOfDayEnum.Morning,
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