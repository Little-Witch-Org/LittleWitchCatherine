using System.Collections;
using _Scripts.Components.Misc;
using _Scripts.Dialog_Ink;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Components._TriggerActivators.ForDialogue
{
    /// <summary>
    /// Uses for putting functions on events in Inspector
    /// </summary>
    //[RequireComponent(typeof(PolygonCollider2D))]
    public class ClickDialogueActivatorForNpc : MonoBehaviour, IClickable
    {

        public void OnHoverEnter()
        {
            //_isMouseOver = true;

            EventManager.Instance.MiscEvents.CursorChangeToHoverOnTrigger();
        }

        public void OnHoverExit()
        {
            //_isMouseOver = false;

            EventManager.Instance.MiscEvents.CursorChangeToDefault();
        }

        public void OnClick()
        {
            StartCoroutine(BlockDialogueInputForFrame());
            //Debug.Log(gameObject.name + " clicked");
            gameObject.transform.GetComponentInParent<StandaloneDialogueComponent>().StartDialogue(false);
            EventManager.Instance.MiscEvents.CursorChangeToDefault();
        }

        public void OnMouseButtonUp()
        {
            EventManager.Instance.MiscEvents.CursorChangeToHoverOnTrigger();
        }
        
        private IEnumerator BlockDialogueInputForFrame()
        {
            EventManager.Instance.InputEvents.SetSubmitLock(true);
            yield return new WaitForEndOfFrame();
            EventManager.Instance.InputEvents.SetSubmitLock(false);
        }
    }
}
