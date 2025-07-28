using System;
using _Scripts.Enums;
using _Scripts.LocationsAndPlaces;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Components.Transition
{
    /// <summary>
    /// Using to check block state and change scene
    /// </summary>
    public class ChangeSceneComponent : MonoBehaviour
    {
        [SerializeField] private SceneNamesEnum scene;
        [SerializeField] private string locationName;
        private bool _isSelected;
        
        public void ChangeSceneTrigger()
        {
            if (EventManager.Instance.TransitionEvents.GetIsTransitionDisabled())
            {
                Debug.Log("Transition disabled");
                return;
            }
            
            if (!_isSelected)
            {
                _isSelected = true;

                if (LocationManager.Instance.IsLocationLocked(locationName))
                {
                    //Debug.Log(selectedPlace + " is locked");
                    _isSelected = false;
                    return;
                    //add audio or animation
                }

                EventManager.Instance.TransitionEvents.ChangeScene(scene);
            }
        }
    }
}