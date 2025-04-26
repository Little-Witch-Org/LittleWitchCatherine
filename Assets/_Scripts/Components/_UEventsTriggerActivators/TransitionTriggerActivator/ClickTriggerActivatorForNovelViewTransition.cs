using _Scripts.Components.Misc;
using _Scripts.Components.Transition;
using _Scripts.Components.TransitionComponents;
using UnityEngine;

namespace _Scripts.Components._UEventsTriggerActivators.TransitionTriggerActivator
{
    /// <summary>
    /// Activates transition events.
    /// </summary>
    public class ClickTriggerActivatorForNovelViewTransition : MonoBehaviour, IClickable
    {
        private bool _isTransitionActivated;

        private void OnEnable()
        {
            EventManager.Instance.TransitionEvents.OnPlaceTransitionTrigger += SetTransitionActivated;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.TransitionEvents.OnPlaceTransitionTrigger -= SetTransitionActivated;
        }

        //handle existence of one component - change place or scene
        public void OnClick()
        {
            EventManager.Instance.MiscEvents.CursorChangeToDefault();
            //Debug.Log("Clicked: " + name);
            if (gameObject.TryGetComponent<NovelViewSetPlaceAndLocationToTransitionComponent>(out var novelComponent))
            {
                novelComponent.SelectPlace();
            }
            else if (gameObject.TryGetComponent<ChangeSceneTriggerComponent>(out var sceneTriggerComponent))
            {
                sceneTriggerComponent.ChangeSceneTrigger();
            }
        }

        public void OnMouseButtonUp()
        {
            if (!_isTransitionActivated)
            {
                EventManager.Instance.MiscEvents.CursorChangeToHoverOnTrigger();
                //Debug.Log("Mouse Button Released: " + name);
            }
        }

        public void OnHoverEnter()
        {
            EventManager.Instance.MiscEvents.CursorChangeToHoverOnTrigger();
            //Debug.Log("Hover Entered: " + name);
        }

        public void OnHoverExit()
        {
            EventManager.Instance.MiscEvents.CursorChangeToDefault();
            //Debug.Log("Hover Exited: " + name);
        }

        private void SetTransitionActivated(string location, string place)
        {
            _isTransitionActivated = true;
        }
 
    }
}
