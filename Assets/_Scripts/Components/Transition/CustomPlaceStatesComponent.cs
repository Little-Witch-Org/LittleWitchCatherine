using System;
using System.Collections.Generic;
using _Scripts.Enums;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Components.Transition
{
    /// <summary>
    /// Component with custom sprites for Places. Can use 4 variants of sprites for each timeOfDay or at least 1 variant for custom state. 
    /// </summary>
    public class CustomPlaceStatesComponent : MonoBehaviour
    {
        [Header("ToD for copy to states")]
        #pragma warning disable CS0414 //disable unused private members
        [SerializeField] private string morning = "Morning";
        [SerializeField] private string afternoon = "Afternoon";
        [SerializeField] private string evening = "Evening";
        [SerializeField] private string night = "Night";
        #pragma warning restore CS0414
        
        
        [Serializable]
        private class PlaceState
        {
            public string
                placeStateString; //must contain string in format placeState_TimeOfDay (CustomState1_Morning) //todo add name validator ?

            public Sprite customPlaceStateSprite;
        }
        [Header("Custom Place States")]
        [SerializeField] private List<PlaceState> customStates = new();
        //[SerializeField] private SpriteRenderer placeSpriteRenderer;

        private Dictionary<string, Sprite> _stateSpriteMap;

        private void Awake()
        {
            //placeSpriteRenderer = GetComponent<SpriteRenderer>();

            _stateSpriteMap = new Dictionary<string, Sprite>();
            foreach (var pair in customStates)
            {
                _stateSpriteMap[pair.placeStateString] = pair.customPlaceStateSprite;
            }
        }
        
        public Sprite GetCustomPlaceStateSprite(TimeOfDayEnum timeOfDayEnum, string stateName)
        {
            //string stateKeyBase = stateName;
            string stateKeyComposite = $"{stateName}_{timeOfDayEnum}";

            // Сначала ищем точное совпадение (например, Custom1_Night)
            if (_stateSpriteMap.TryGetValue(stateKeyComposite, out Sprite sprite))
            {
                return sprite;
            }

            // Если точного совпадения нет, ищем по шаблону в порядке времени суток
            TimeOfDayEnum[] searchOrder = 
            {
                TimeOfDayEnum.Morning,
                TimeOfDayEnum.Afternoon,
                TimeOfDayEnum.Evening,
                TimeOfDayEnum.Night
            };

            foreach (var time in searchOrder)
            {
                string keyToCheck = $"{stateName}_{time}";
                if (_stateSpriteMap.TryGetValue(keyToCheck, out sprite))
                {
                    Debug.Log($"Set fallback sprite for {stateName} (using {time} variant)");
                    return sprite;
                    
                }
            }

            // Если вообще ничего не нашли
            Debug.LogWarning($"No sprite found for state {stateName} (any time of day)");
            return null;
        }
        

        /*public void SetCustomPlaceStateSprite(TimeOfDayEnum timeOfDayEnum, string stateName)
        {
            // enum to string
            string stateKeyBase = stateName;
            string stateKeyComposite = stateKeyBase;

            //add _timeOfDay
            switch (timeOfDayEnum)
            {
                case TimeOfDayEnum.Morning:
                {
                    stateKeyComposite += "_Morning";
                    break;
                }
                case TimeOfDayEnum.Afternoon:
                {
                    stateKeyComposite += "_Afternoon";
                    break;
                }
                case TimeOfDayEnum.Evening:
                {
                    stateKeyComposite += "_Evening";
                    break;
                }
                case TimeOfDayEnum.Night:
                {
                    stateKeyComposite += "_Night";
                    break;
                }
            }


            // find stateString in dictionary
            if (_stateSpriteMap.TryGetValue(stateKeyComposite, out Sprite sprite))
            {
                placeSpriteRenderer.sprite = sprite;
            }
            else
            {
                foreach (var pair in _stateSpriteMap)
                {
                    if (pair.Key.Equals(stateKeyBase + "_Morning"))
                    {
                        placeSpriteRenderer.sprite = pair.Value;
                        Debug.Log("set morning");
                    }
                    else if (pair.Key.Equals(stateKeyBase + "_Afternoon"))
                    {
                        placeSpriteRenderer.sprite = pair.Value;
                        Debug.Log("set afternoon");
                    }
                    else if (pair.Key.Equals(stateKeyBase + "_Evening"))
                    {
                        placeSpriteRenderer.sprite = pair.Value;
                        Debug.Log("set evening");
                    }
                    else if (pair.Key.Equals(stateKeyBase + "_Night"))
                    {
                        placeSpriteRenderer.sprite = pair.Value;
                        Debug.Log("set night");
                    }
                    else
                    {
                        Debug.LogWarning($"No sprite for state {stateKeyBase}");
                    }
                }
            }
        }*/
    }
}