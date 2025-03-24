using _Scripts.Components.TimeManagement.Enums;
using UnityEngine;

namespace _Scripts.ScriptableObjects.Locations
{
    
    public abstract class LocationStateSo : ScriptableObject
    {
        [SerializeField] protected string locationName;

        public string LocationName
        {
            get => locationName;
            set => locationName = value;
        }

        public TimeOfDay TimeOfDay
        {
            get => timeOfDay;
            set => timeOfDay = value;
        }
        //public string[] charactersInLocation;

        [SerializeField] protected TimeOfDay timeOfDay;

    }
    
    
}