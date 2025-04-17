using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _Scripts.Components.UEventsTriggers
{
    /// <summary>
    /// Uses for putting functions on events in Inspector
    /// //todo not sure that using  EventSystem.current.IsPointerOverGameObject() is good idea to prevent clicks then UI opened. It also blocks other colliders
    /// </summary>
    //[RequireComponent(typeof(PolygonCollider2D))]
    public class ClickTriggerActivatorForNpc : MonoBehaviour
    {
        public UnityEvent OnClick;
        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        private bool _isClicked; //handle double click

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
        
            // block click for this frame (to handle simultaneous click in dialog's first string)
            StartCoroutine(BlockDialogueInputForFrame());
            OnClick.Invoke();
        }

        private IEnumerator BlockDialogueInputForFrame()
        {
            EventManager.Instance.InputEvents.LockSubmit(true);
            yield return new WaitForEndOfFrame();
            EventManager.Instance.InputEvents.LockSubmit(false);
        }

        private void OnMouseExit()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            OnExit.Invoke();
        }

        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            OnEnter.Invoke();
        }
    }
}
