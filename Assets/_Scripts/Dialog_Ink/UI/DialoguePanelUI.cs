using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Dialog_Ink.UI
{
    public class DialoguePanelUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject contentParent;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private DialogueChoiceButton[] choiceButtons;
        
        [SerializeField] private CanvasGroup canvasGroup;

        private void Awake()
        {
          contentParent.SetActive(false);
          
          ResetPanel();
        }

        private void OnEnable()
        {
            EventManager.Instance.DialogueEvents.OnDialogueStarted += DialogueStarted;
            EventManager.Instance.DialogueEvents.OnDialogueFinished += DialogueFinished;
            EventManager.Instance.DialogueEvents.OnDisplayDialogue += DisplayDialogue;
        }
        private void OnDisable()
        {
            EventManager.Instance.DialogueEvents.OnDialogueStarted -= DialogueStarted;
            EventManager.Instance.DialogueEvents.OnDialogueFinished -= DialogueFinished;
            EventManager.Instance.DialogueEvents.OnDisplayDialogue -= DisplayDialogue;
        }
        
        
        private void DialogueStarted()
        {
            
            contentParent.SetActive(true);
        }
        
        private void DialogueFinished()
        {
            
            contentParent.SetActive(false);
            
            //reset anything for nex time
            ResetPanel();
        }

        private void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
        {
            dialogueText.text = dialogueLine;
            
            //save mouse position
            Vector2 mousePos = Input.mousePosition;
            
            //reset button 
            EventSystem.current.SetSelectedGameObject(null);
            
            //if there are more choices coming that we can support -> log an error
            if (dialogueChoices.Count > choiceButtons.Length)
            {
                Debug.LogError("More dialogue choices ("
                               +dialogueChoices.Count + ") than choices we can support("
                               + choiceButtons.Length + ")");
            }
            
            // start with all of the choice buttons hidden
            foreach (DialogueChoiceButton choiceButton in choiceButtons) 
            {
                choiceButton.gameObject.SetActive(false);
            }

            // enable and set info for buttons depending on ink choice information (choices (indexes) are going from up to down, but button indexes are revers)
            int choiceButtonIndex = dialogueChoices.Count - 1;
            for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
            {
                Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
                DialogueChoiceButton choiceButton = choiceButtons[choiceButtonIndex];

                choiceButton.gameObject.SetActive(true);
                choiceButton.SetChoiceText(dialogueChoice.text);
                choiceButton.SetChoiceIndex(inkChoiceIndex);

                //if (inkChoiceIndex == 0)
                //{
                //    choiceButton.SelectButton(); //disable auto select button
                //    EventManager.Instance.DialogueEvents.UpdateChoiceIndex(0);
                //}
                
                choiceButtonIndex--;
            }
            //restore highlite down the cursor
            StartCoroutine(RestoreSelectionAfterFrame(mousePos));
        }

        private IEnumerator RestoreSelectionAfterFrame(Vector2 mousePosition)
        {
            yield return null;
    
            var pointerData = new PointerEventData(EventSystem.current) {
                position = mousePosition,
                button = PointerEventData.InputButton.Left
            };
    
            foreach (var button in choiceButtons)
            {
                if (!button.gameObject.activeSelf) continue;
        
                var rectTransform = button.GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition, null))
                {
                    // only highlight without selection
                    ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerEnterHandler);
                    
                    break;
                }
            }
    
            // reset select state
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void ResetPanel()
        {
            dialogueText.text = "";
        }
        
    }
}