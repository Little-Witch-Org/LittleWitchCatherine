using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Components.SceneTransitionComponents
{
    /// <summary>
    /// Component stores sprites of novel locations and changes them depending on game stateEnum
    /// </summary>
    public class NovelViewPlaceStateComponent : MonoBehaviour
    {
        [SerializeField] private string placeName;
        
        
        [FormerlySerializedAs("roomStateSprites")] [SerializeField] private List<Sprite> placeStateSprites;
        
        
        private void Start()
        {
            placeName = gameObject.name.Split("(Clone)")[0]; //get name from place prefab and set as placeName
            
            
            if (LocationManager.Instance == null)
            {
                Debug.LogError("NovelViewPlaceStateComponent can't find LocationManager");
                return;
            }

            TimeOfDay timeOfDay = LocationManager.Instance.GetLocationTimeOfDayState(placeName);
            LoadRoomState(timeOfDay);
        }
        
        
        private void LoadRoomState(TimeOfDay timeOfDay)
        {
            timeOfDay = TimeManager.Instance.timeOfDay;

            switch (timeOfDay)
            {
                case TimeOfDay.Morning:
                {
                    GetComponent<SpriteRenderer>().sprite = placeStateSprites[0];
                    break;
                }
                case TimeOfDay.Afternoon:
                {
                    GetComponent<SpriteRenderer>().sprite = placeStateSprites[1];
                    break;
                }
                case TimeOfDay.Evening:
                {
                    GetComponent<SpriteRenderer>().sprite = placeStateSprites[2];
                    break;
                }
                case TimeOfDay.Night:
                {
                    GetComponent<SpriteRenderer>().sprite = placeStateSprites[3];
                    break;
                }
            }
        
            //GetComponent<SpriteRenderer>().sprite = placeStateSprites[0];
        }


    }
}
