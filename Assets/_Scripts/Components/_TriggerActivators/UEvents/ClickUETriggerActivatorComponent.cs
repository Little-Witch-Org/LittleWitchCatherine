using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _Scripts.Components._TriggerActivators.UEvents
{
    public class ClickUETriggerActivatorComponent:MonoBehaviour
    {
        public UnityEvent onClick;
        
        private void OnMouseDown()
        {
            onClick?.Invoke();
        }
        
        
        //old code from old triggers
        //public UnityEvent OnClick;
        //public UnityEvent OnEnter;
        //public UnityEvent OnExit;

        //private bool _isClicked; //handle double click
        //private bool _isMouseOver;
        
        /*private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // block click for this frame (to handle simultaneous click in dialog's first string)
            StartCoroutine(BlockDialogueInputForFrame());
            OnClick.Invoke();
            EventManager.Instance.MiscEvents.CursorChangeToDefault();
        }

        private IEnumerator BlockDialogueInputForFrame()
        {
            EventManager.Instance.InputEvents.SetSubmitLock(true);
            yield return new WaitForEndOfFrame();
            EventManager.Instance.InputEvents.SetSubmitLock(false);
        }

        private void OnMouseUp()
        {
            if (EventSystem.current.IsPointerOverGameObject() || !_isMouseOver)
            {
                return;
            }

            EventManager.Instance.MiscEvents.CursorChangeToHoverOnTrigger();
        }

        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            _isMouseOver = true;

            EventManager.Instance.MiscEvents.CursorChangeToHoverOnTrigger();
        }

        private void OnMouseExit()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            _isMouseOver = false;

            EventManager.Instance.MiscEvents.CursorChangeToDefault();
        }*/
    }
}