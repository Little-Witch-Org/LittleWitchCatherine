using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Components.SceneTransitionComponents
{
    /// <summary>
    /// Component stores sprites of novel locations and changes them depending on game stateEnumTest
    /// </summary>
    public class NovelViewPlaceStateComponent : MonoBehaviour
    {
        [SerializeField] private List<Sprite> roomStateSprites;
        
        private void LoadRoomState(TimeOfDay timeOfDay)
        {
            timeOfDay = TimeManagerTurnBased.Instance.timeOfDay;

            switch (timeOfDay)
            {
                case TimeOfDay.Morning:
                {
                    GetComponent<SpriteRenderer>().sprite = roomStateSprites[0];
                    break;
                }
                case TimeOfDay.Afternoon:
                {
                    GetComponent<SpriteRenderer>().sprite = roomStateSprites[1];
                    break;
                }
                case TimeOfDay.Evening:
                {
                    GetComponent<SpriteRenderer>().sprite = roomStateSprites[2];
                    break;
                }
                case TimeOfDay.Night:
                {
                    GetComponent<SpriteRenderer>().sprite = roomStateSprites[3];
                    break;
                }
            }
        
            //GetComponent<SpriteRenderer>().sprite = roomStateSprites[0];
        }

        private void Start()
        {
            TimeOfDay timeOfDay = LocationManager.Instance.GetCurrentLocation().TimeOfDay;
            LoadRoomState(timeOfDay);
        }
    }
}
