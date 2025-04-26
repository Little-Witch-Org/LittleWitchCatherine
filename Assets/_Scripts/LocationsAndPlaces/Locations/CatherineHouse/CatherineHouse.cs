using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse;

namespace _Scripts.LocationsAndPlaces.Locations.CatherineHouse
{
    public class CatherineHouse : Location
    {
        public CatherineHouse() : base("CatherineHouse", TimeOfDay.Morning,
            new List<Place>
            {
                new Kitchen(),
                new AtticRoom(),
                new BasementLab(),
                new BelcroftCabinet(),
                new DiningRoom(),
                new FFCorridor(),
                new ForemtogRoom(),
                new GuestBedroom(),
                new KatherineRoom(),
                new KatherineDressingRoom(),
                new LibraryFirstRoom(),
                new LibrarySecondRoom(),
                new LivingRoom(),
                new ParentsRoom(),
                new SFCorridor(),
                new StorageUnderStairs(),
                new TFCorridor()
            }
        )
        {
            //empty constructor
        }
    }

}