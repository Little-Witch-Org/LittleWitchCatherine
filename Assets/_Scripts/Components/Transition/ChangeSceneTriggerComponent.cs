using System;
using _Scripts.LocationsAndPlaces;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Components.Transition
{
    public class ChangeSceneTriggerComponent : MonoBehaviour
    {
        [SerializeField] private SceneNames scene;
        [SerializeField] private string _locationName;
        private bool _isSelected;
        
        public void ChangeSceneTrigger()
        {
            if (!_isSelected)
            {
                _isSelected = true;

                if (LocationManager.Instance.IsLocationLocked(_locationName))
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