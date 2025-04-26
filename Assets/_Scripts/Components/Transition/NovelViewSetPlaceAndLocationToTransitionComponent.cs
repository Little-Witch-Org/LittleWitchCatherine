using System;
using System.Collections.Generic;
using _Scripts.Enums.Places;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Components.TransitionComponents
{
    /// <summary>
    /// Setting the location and place to transit using event system. (need to handle several maps case. No it is only CatherineHouseMap). add new dropdown?
    /// Uses with change scene trigger on map to enter in current location. (need NovelViewChangePlaceAfterTransitionComponent to perform place change)
    /// </summary>
    public class NovelViewSetPlaceAndLocationToTransitionComponent : MonoBehaviour
    {
        private bool _isSelected = false; //handle multiple clicks (if is locked -> can invoke method multiple times)
        
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
            if (!_isSelected)
            {
                _isSelected = true;

                if (LocationManager.Instance.IsPlaceLocked(selectedPlace))
                {
                    //Debug.Log(selectedPlace + " is locked");
                    _isSelected = false;
                    return;
                    //add audio or animation
                }
                
                //Debug.Log(selectedPlace + " is selected");
                EventManager.Instance.TransitionEvents.PlaceTransitionTrigger(locationType.ToString(), selectedPlace);
            }
            /*Debug.Log("invoke event for "+ locationType);
            Debug.Log("invoke event for "+ selectedPlace);
            //invoke event here
            Debug.Log($"Selected {locationType} Place: {selectedPlace}");*/
        }
    }

}