using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Components.Transition
{
    /// <summary>
    /// Component stores sprites of novel locations and changes them depending on game stateEnum
    /// //todo add lists with custom files -> list<custom state> with sprites ? add logic to current place class (customState/boll - on/off). Then location manager listens event
    /// //todo "OnPlaceStateChange<bool(on/off),string(state)>" then find in places "state" field and toggle it. Than before load place this component gets from location manager -> locations -> places "state" for current loading place
    /// </summary>
    public class NovelViewPlaceStateComponent : MonoBehaviour
    {
        [SerializeField] private string placeName;
        
        
        [FormerlySerializedAs("roomStateSprites")] [SerializeField] private List<Sprite> placeStateSprites;
        
        private SpriteRenderer _spriteRenderer;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            placeName = gameObject.name.Replace("(Clone)", ""); //get name from place prefab and set as placeName
            
            
            if (LocationManager.Instance == null)
            {
                Debug.LogError("NovelViewPlaceStateComponent can't find LocationManager");
                return;
            }

            TimeOfDayEnum timeOfDayEnum = LocationManager.Instance.GetLocationTimeOfDayState(placeName);
            LoadRoomState(timeOfDayEnum);
            EventManager.Instance.TransitionEvents.PlaceSpriteChanged(_spriteRenderer);
        }
        
        
        private void LoadRoomState(TimeOfDayEnum timeOfDayEnum)
        {
            timeOfDayEnum = TimeManager.Instance.timeOfDayEnum;

            switch (timeOfDayEnum)
            {
                case TimeOfDayEnum.Morning:
                {
                    GetComponent<SpriteRenderer>().sprite = placeStateSprites[0];
                    break;
                }
                case TimeOfDayEnum.Afternoon:
                {
                    GetComponent<SpriteRenderer>().sprite = placeStateSprites[1];
                    break;
                }
                case TimeOfDayEnum.Evening:
                {
                    GetComponent<SpriteRenderer>().sprite = placeStateSprites[2];
                    break;
                }
                case TimeOfDayEnum.Night:
                {
                    GetComponent<SpriteRenderer>().sprite = placeStateSprites[3];
                    break;
                }
            }
        
            //GetComponent<SpriteRenderer>().sprite = placeStateSprites[0];
        }


    }
}
