using System;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces.Maps;
using _Scripts.LocationsAndPlaces.Maps.CatherineHouse;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace _Scripts.Managers
{
    public class MapViewSpritesChanger : MonoBehaviour
    {
        //public static MapViewSpritesChanger Instance;
        [SerializeField] private Map currentMap;


        //private void Awake()
        //{
        //    if (Instance == null)
        //    {
        //        Instance = this;
        //        DontDestroyOnLoad(gameObject);
        //    }
        //    else
        //    {
        //        Destroy(gameObject);
        //    }
        //}

        //private void Start()
        //{
        //    MapSpritesChange(TimeManager.Instance.timeOfDayEnum);
        //}
        
        private void OnEnable()
        {
            EventManager.Instance.TimeEvents.OnUpdateTimeOfDay += MapSpritesChange;//todo handle
            EventManager.Instance.TransitionEvents.OnLoadedMap += UpdateMapConditions;
        }
        private void OnDisable()
        {
            EventManager.Instance.TimeEvents.OnUpdateTimeOfDay -= MapSpritesChange;
            EventManager.Instance.TransitionEvents.OnLoadedMap -= UpdateMapConditions;
        }


        private void UpdateMapConditions(Map map)
        {
            //Debug.Log("update conditions");
            //Debug.Log(map.GetTexturesSo(TimeManager.Instance.timeOfDayEnum).ground);
            currentMap = map;
            MapSpritesChange(TimeManager.Instance.timeOfDayEnum);
        }
        

        private void MapSpritesChange(TimeOfDayEnum timeOfDayEnum)
        { 
            
            //Debug.Log("MapSpritesChange triggered");
            //->check current map in char manager

            switch (timeOfDayEnum)
            {
                case TimeOfDayEnum.Morning:
                {
                    ReplaceSpritesByTimeOfDay(currentMap, TimeOfDayEnum.Morning);
                    break;
                }
                case TimeOfDayEnum.Afternoon:
                {
                    ReplaceSpritesByTimeOfDay(currentMap, TimeOfDayEnum.Afternoon);
                    break;
                }
                case TimeOfDayEnum.Evening:
                {
                    ReplaceSpritesByTimeOfDay(currentMap, TimeOfDayEnum.Evening);
                    break;
                }
                case TimeOfDayEnum.Night:
                {
                    ReplaceSpritesByTimeOfDay(currentMap, TimeOfDayEnum.Night);
                    break;
                }
            }
        }
        


        private void ReplaceSpritesByTimeOfDay(Map map,TimeOfDayEnum timeOfDayEnum)
        {
            //replace ground
            map.GetGround().gameObject.GetComponent<SpriteRenderer>().sprite =
                map.GetTexturesSo(timeOfDayEnum).ground;
            
            //get lists of map objects and sprites
            List<GameObject> mapObjects = map.GetMapObjects();
            List<Sprite> sprites = map.GetTexturesSo(timeOfDayEnum).mapObjectsSprites;
            
            //create dictionary with names (before '.') and sprites
            Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
            foreach (Sprite sprite in sprites)
            {
                string spriteName = sprite.name;
                string number = spriteName.Split('.')[0];
                spriteDict[number] = sprite;
            }
            
            //replace sprites in objects depending on names (before '.')
            foreach (GameObject obj in mapObjects)
            {
                if (obj == null) continue;

                string objName = obj.name;
                string objNumber = objName.Split('.')[0];

                // checks
                if (spriteDict.ContainsKey(objNumber))
                {
                    SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
                    if (renderer != null)
                    {
                        renderer.sprite = spriteDict[objNumber];
                        //Debug.Log($"Replace sprite on object {obj.name} to {spriteDict[objNumber].name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Object {obj.name} has no SpriteRenderer");
                    }
                }
                else
                {
                    Debug.LogWarning($"There is no sprite with name {objNumber} was found for object {obj.name}");
                }
            }
        }
    }
}