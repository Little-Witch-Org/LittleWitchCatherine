using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse;

namespace _Scripts.LocationsAndPlaces
{
    /// <summary>
    /// Stores location info.
    /// Store list of places.
    /// Location manager check current state of locations and places and share this info with place prefabs.
    /// </summary>
    public abstract class Location
    {
        public string LocationName { get; protected set; }
        public TimeOfDay TimeOfDay { get;  set; }
        public List<Place> Places { get; protected set; }

        public bool isLocationLocked{ get; set; }

        protected Location(string name, TimeOfDay timeOfDay, List<Place> places)
        {
            LocationName = name;
            TimeOfDay = timeOfDay;
            Places = places;
        }

    }
    
}