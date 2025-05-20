using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.Bathhouse;
using _Scripts.LocationsAndPlaces.Places.Seesaw;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class Bathhouse : Location
    {
        public Bathhouse() : base("Bathhouse", TimeOfDayEnum.Morning,
            new List<Place>
            {
                new EnterInBathhouse(),
                new Bath(),
                new Barn()
            }
        )
        {
            //empty constructor
        }

    }
}