using System;
using System.Collections;
using _Scripts.Characters.NPC;
using _Scripts.Components.Misc;
using _Scripts.Dialog_Ink;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Components._TriggerActivators.ForDialogue
{
    /// <summary>
    /// Uses for activating dialogue with npc by click on game object
    /// </summary>
    //[RequireComponent(typeof(PolygonCollider2D))]
    //[RequireComponent(typeof(StandaloneDialogueComponent))]
    public class ClickDialogueActivatorForNpc : MonoBehaviour, IClickable
    {
        [Header("Dialogue Activator Settings")]
        [Header("Npc bind for current activator")]
        [SerializeField] private string npcName; //used for dialogue event
        
        //uses for set and launch custom dialogue by click
        [Header("custom dialogue settings")]
        [SerializeField] private string customDialogueKnot;
        [SerializeField] private bool isUsingCustomDialogue;


        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            
            var standalone = gameObject.transform.parent.GetComponent<NpcCharAbstract>();
            if (standalone != null)
            {
                npcName = standalone.GetNpcName();
            }
            
            if (String.IsNullOrEmpty(npcName))
            {
                Debug.LogError($"Npc name not assigned for activator {gameObject.name}");
            }
        }


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
            //gameObject.transform.GetComponentInParent<StandaloneDialogueComponent>().StartDialogue(false);
            
            if (isUsingCustomDialogue)
            {
                EventManager.Instance.DialogueEvents.SetCustomDialogueKnot(npcName,customDialogueKnot);
            }
            
            EventManager.Instance.DialogueEvents.StartDialogueWithNpc(npcName);
            EventManager.Instance.MiscEvents.CursorChangeToDefault();
        }

        public void OnMouseButtonUp()
        {
            EventManager.Instance.MiscEvents.CursorChangeToHoverOnTrigger();
        }
        
        private IEnumerator BlockDialogueInputForFrame()
        {
            EventManager.Instance.InputEvents.SetSubmitActive(false);
            yield return new WaitForEndOfFrame();
            EventManager.Instance.InputEvents.SetSubmitActive(true);
        }
    }
}
