using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Components.TransitionComponents
{
    /// <summary>
    /// Setting the location and place to transit using event system. (need to handle several maps case. No it is only CatherineHouseMap). add new dropdown?
    /// </summary>
    public class NovelViewSetPlaceAndLocationToTransitionComponent : MonoBehaviour
    {
        
        // field for storing the selected location
        [SerializeField] private CatherineHouseMap locationType;

        // The field for storing the selected room (as a string, since the type is dynamic)
        [SerializeField] private string selectedPlace;



        // Dictionary  of matching locations and places
        public Dictionary<CatherineHouseMap, Type> locationTypeEnums = new Dictionary<CatherineHouseMap, Type>
        {
            { CatherineHouseMap.CatherineHouse, typeof(CatherineHouseNovelViewPlaces) },
            { CatherineHouseMap.Greenhouse, typeof(GreenhouseNovelViewPlaces) },
            { CatherineHouseMap.Seesaw, typeof(SeesawNovelViewPlaces) },
            // todo new matching locations/places
        };

        
        public void SelectPlace()
        {
            EventManager.Instance.TransitionEvents.TriggerTransition(locationType.ToString(), selectedPlace);
            
            /*Debug.Log("invoke event for "+ locationType);
            Debug.Log("invoke event for "+ selectedPlace);
            //invoke event here
            Debug.Log($"Selected {locationType} Place: {selectedPlace}");*/
        }
    }

}