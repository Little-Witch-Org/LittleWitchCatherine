using System;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.LocationsAndPlaces.Maps
{
    /// <summary>
    /// Stores info about map (with custom states). Contains SO's with sprites (map manager must control this). Contains links to game objects on map.
    /// </summary>
    public abstract class Map:MonoBehaviour
    {
        protected string MapName;
        protected bool IsLocked;
        
        [SerializeField] protected MapTexturesSo texturesSoMorning;
        [SerializeField] protected MapTexturesSo texturesAfternoon;
        [SerializeField] protected MapTexturesSo texturesSoEvening;
        [SerializeField] protected MapTexturesSo texturesSoNight;
        [SerializeField] protected Transform mapObjects;
        [SerializeField] protected GameObject ground;
        [SerializeField] protected GameObject filter;


        public MapTexturesSo GetTexturesSo(TimeOfDayEnum timeOfDayEnum)
        {
            switch (timeOfDayEnum)
            {
                case TimeOfDayEnum.Morning:
                {
                    return texturesSoMorning;
                }
                case TimeOfDayEnum.Afternoon:
                {
                    return texturesAfternoon;
                }
                case TimeOfDayEnum.Evening:
                {
                    return texturesSoEvening;
                }
                case TimeOfDayEnum.Night:
                {
                    return texturesSoNight;
                }
                
            }
            return null;
        }
        
        public List<GameObject> GetMapObjects()
        {

            List<GameObject> childList = new List<GameObject>();
    
            foreach (Transform child in mapObjects)
            {
                if (child != null) // На всякий случай
                {
                    childList.Add(child.gameObject);
                }
            }

            return childList;
        }

        public GameObject GetGround()
        {
            return ground;
        }

        public GameObject GetFilter()
        {
            return filter;
        }
    }
}