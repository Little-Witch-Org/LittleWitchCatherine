using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Places;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse;

namespace _Scripts.LocationsAndPlaces.Locations
{
    /// <summary>
    /// Stores location info.
    /// Store list of places.
    /// Location manager check current state of locations and places and share this info with place prefabs.
    /// </summary>
    public abstract class Location
    {
        public string LocationName { get; protected set; }
        public TimeOfDayEnum TimeOfDayEnum { get;  set; }
        public List<Place> Places { get; protected set; }

        public bool isLocationLocked{ get; set; }

        protected Location(string name, TimeOfDayEnum timeOfDayEnum, List<Place> places)
        {
            LocationName = name;
            TimeOfDayEnum = timeOfDayEnum;
            Places = places;
        }

    }
    
}