using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Components.TransitionComponents
{
    /// <summary>
    /// Setting the location and place to transit using event system
    /// </summary>
    public class NovelViewSetPlaceAndLocationToTransitionComponent : MonoBehaviour
    {
        
        // field for storing the selected location
        [SerializeField] private LocationType locationType;

        // The field for storing the selected room (as a string, since the type is dynamic)
        [SerializeField] private string selectedPlace;

        // Enum for locations //todo add separate enum for locations ?
        public enum LocationType
        {
            CatherineHouse,
            Greenhouse,
            Seesaw
            //todo  new locations
        }

        // Dictionary  of matching locations and places
        public Dictionary<LocationType, Type> locationTypeEnums = new Dictionary<LocationType, Type>
        {
            { LocationType.CatherineHouse, typeof(CatherineHouseNovelViewPlaces) },
            { LocationType.Greenhouse, typeof(GreenhouseNovelViewPlaces) },
            { LocationType.Seesaw, typeof(GreenhouseNovelViewPlaces) },
            // todo new matching locations/places
        };

        
        public void SelectPlace()
        {
            EventManager.Instance.transitionEvents.TriggerTransition(locationType.ToString(), selectedPlace);
            
            /*Debug.Log("invoke event for "+ locationType);
            Debug.Log("invoke event for "+ selectedPlace);
            //invoke event here
            Debug.Log($"Selected {locationType} Place: {selectedPlace}");*/
        }
    }

}