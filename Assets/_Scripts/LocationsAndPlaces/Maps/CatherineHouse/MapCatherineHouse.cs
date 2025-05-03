using System;
using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.LocationsAndPlaces.Maps.CatherineHouse
{
    public class MapCatherineHouse:Map
    {
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            MapName = "CatherineHouseMap";
            EventManager.Instance.TransitionEvents.LoadedMap(this);
            //Debug.Log("MapCatherineHouse Initialized");
        }
    }
}