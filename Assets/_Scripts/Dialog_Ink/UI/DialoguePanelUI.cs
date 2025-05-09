using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Managers;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.Dialog_Ink.UI
{
    public class DialoguePanelUI : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private float typingSpeed = 0.04f;
        private Coroutine _typingCoroutine;
        private bool isSubmitPressed;
        
        [Header("Components")]
        [SerializeField] private GameObject contentParent;
        [SerializeField] private GameObject background;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private GameObject continueIcon;
        [SerializeField] private DialogueChoiceButton[] choiceButtons;
        private readonly List<DialogueChoiceButton> _activeChoiceButtons = new List<DialogueChoiceButton>();//uses to hide/show while text typing and after
        
        [SerializeField] private CanvasGroup canvasGroup;
        
        [SerializeField] private TMP_Text speaker1Text;
        [SerializeField] private TMP_Text speaker2Text;
        [SerializeField] private Image portrait1Image;
        [SerializeField] private Image portrait2Image;
        
        [Header("Components Cutscene")]
        [SerializeField] private GameObject contentParentCutscene;
        [SerializeField] private TMP_Text dialogueTextCutscene;
        
        //Player portraits //todo make load to lists automatically from resources?
        [SerializeField] private List<Sprite> playerPortraitsSpriteList = new List<Sprite>();
        
        //Npc portraits
        [SerializeField] private List<Sprite> npcPortraitsSpriteList = new List<Sprite>();
        
        
        private Dictionary<string, Dictionary<string, Sprite>> _characterPortraits; // 
        
        //create dictionary of characters and their sprite portraits
        private void InitializePortraitsDictionary()
        {
            Dictionary<string, Sprite> innerPlayerPortraitsDictionary = new Dictionary<string, Sprite>();
            foreach (var sprite in playerPortraitsSpriteList)
            {   
                innerPlayerPortraitsDictionary.Add(sprite.name, sprite);
            }
            
            Dictionary<string, Sprite> innerNPCPortraitsDictionary = new Dictionary<string, Sprite>();
            foreach (var sprite in npcPortraitsSpriteList)
            {   
                innerNPCPortraitsDictionary.Add(sprite.name, sprite);
            }
            
            _characterPortraits = new Dictionary<string, Dictionary<string, Sprite>>();
            _characterPortraits.Add("player", innerPlayerPortraitsDictionary);
            _characterPortraits.Add("npc", innerNPCPortraitsDictionary);
        }

        private void Awake()
        {
          contentParent.SetActive(false);
          contentParentCutscene.SetActive(false);
          
          ResetPanel();
          
          InitializePortraitsDictionary();
        }

        private void OnEnable()
        {
            EventManager.Instance.DialogueEvents.OnDialogueStarted += DialogueStarted;
            EventManager.Instance.DialogueEvents.OnDialogueStartedCutscene += DialogueStartedCutscene;
            EventManager.Instance.DialogueEvents.OnDialogueFinished += DialogueFinished;
            EventManager.Instance.DialogueEvents.OnDialogueFinishedCutscene += DialogueFinishedCutscene;
            EventManager.Instance.DialogueEvents.OnDisplayDialogue += DisplayDialogue;
            EventManager.Instance.CutsceneEvents.OnCutsceneStarted += DisableBackground;
            EventManager.Instance.CutsceneEvents.OnCutsceneFinished += EnableBackground;
            EventManager.Instance.InputEvents.OnSubmitPressed += SubmitPressed;
            
            //tags
            EventManager.Instance.DialogueEvents.OnTagChangeSpeaker1Name += ChangeSpeaker1Name;
            EventManager.Instance.DialogueEvents.OnTagChangeSpeaker2Name += ChangeSpeaker2Name;
            EventManager.Instance.DialogueEvents.OnTagChangePortrait1 += ChangePortrait1;
            EventManager.Instance.DialogueEvents.OnTagChangePortrait2 += ChangePortrait2;
            EventManager.Instance.DialogueEvents.OnTagChangeCurrentSpeaker += ChangeCurrentSpeaker;
            
        }
        private void OnDisable()
        {
            EventManager.Instance.DialogueEvents.OnDialogueStarted -= DialogueStarted;
            EventManager.Instance.DialogueEvents.OnDialogueStartedCutscene -= DialogueStartedCutscene;
            EventManager.Instance.DialogueEvents.OnDialogueFinished -= DialogueFinished;
            EventManager.Instance.DialogueEvents.OnDialogueFinishedCutscene -= DialogueFinishedCutscene;
            EventManager.Instance.DialogueEvents.OnDisplayDialogue -= DisplayDialogue;
            EventManager.Instance.CutsceneEvents.OnCutsceneStarted -= DisableBackground;
            EventManager.Instance.CutsceneEvents.OnCutsceneFinished -= EnableBackground;
            
            //tags
            EventManager.Instance.DialogueEvents.OnTagChangeSpeaker1Name -= ChangeSpeaker1Name;
            EventManager.Instance.DialogueEvents.OnTagChangeSpeaker2Name -= ChangeSpeaker2Name;
            EventManager.Instance.DialogueEvents.OnTagChangePortrait1 -= ChangePortrait1;
            EventManager.Instance.DialogueEvents.OnTagChangePortrait2 -= ChangePortrait2;
            EventManager.Instance.DialogueEvents.OnTagChangeCurrentSpeaker -= ChangeCurrentSpeaker;
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
        private void DialogueStartedCutscene()
        {
            
            contentParentCutscene.SetActive(true);
        }
        
        private void DialogueFinishedCutscene()
        {
            
            contentParentCutscene.SetActive(false);
            
            //reset anything for nex time
            ResetPanel();
        }

        private void HideActiveButtons()
        {
            //Debug.Log(_activeChoiceButtons!=null);
            //Debug.Log(_activeChoiceButtons.Count);
            if (_activeChoiceButtons!=null && _activeChoiceButtons.Count != 0)
            {
                foreach (var button in _activeChoiceButtons)
                {
                    button.gameObject.SetActive(false);
                }
            }
        }

        private void ShowActiveButtons()
        {
            if (_activeChoiceButtons!=null && _activeChoiceButtons.Count != 0)
            {
                foreach (var button in _activeChoiceButtons)
                {
                    button.gameObject.SetActive(true);
                }
            }
        }

        private void SubmitPressed(InputEventContext context)
        {
            if (context == InputEventContext.TypingLine)
            {
                isSubmitPressed = true;
            }
        }
        

        //displays alphabetically (char to char)  //todo to handle end line overflow (type by words ? use maxVisibleCharacters ?)
        private IEnumerator DisplayTypingLine(string line, bool isCutscene)
        {
            //save mouse position
            Vector2 mousePos = Input.mousePosition;
            
            //hide buttons while typing
            HideActiveButtons();
            
            //hide continue icon
            continueIcon.SetActive(false);
            
            //set context ty "typing" to prevent line skipping
            EventManager.Instance.InputEvents.ChangeInputEventContext(InputEventContext.TypingLine);
            
            
            TMP_Text localTextVar;
            if (isCutscene)
            {
                localTextVar = dialogueTextCutscene;
            }
            else
            {
                localTextVar = dialogueText;
            }
            
            //check rtt existence
            bool isAddingRichTextTag = false;
            
            //empty text
            localTextVar.text = "";
            
            //typing char by char
            foreach (var letter in line.ToCharArray())
            {
                //skip typing "animation" if submit pressed
                if (isSubmitPressed)
                {
                    localTextVar.text = line;
                    break;
                }

                //check for rtt, if found, add without waiting
                if (letter == '<' || isAddingRichTextTag)
                {
                    isAddingRichTextTag = true;
                    localTextVar.text += letter;
                    if (letter == '>')
                    {
                        isAddingRichTextTag = false;
                    }
                }
                else
                {
                    localTextVar.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }


            }

            //reset submit state
            isSubmitPressed = false;
            
            //restore context
            EventManager.Instance.InputEvents.ChangeInputEventContext(InputEventContext.Dialogue);
            //show buttons after typing
            ShowActiveButtons();
            
            //show continue icon
            continueIcon.SetActive(true);
            
            //restore highlight down the cursor
            StartCoroutine(RestoreSelectionAfterFrame(mousePos));
        }

        private void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices, bool isCutsceneUI)
        {
            //if (isCutsceneUI)
            //{
            //    dialogueTextCutscene.text = dialogueLine;
            //}
            //else
            //{
            //    dialogueText.text = dialogueLine;
            //}
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
            }
            _typingCoroutine = StartCoroutine(DisplayTypingLine(dialogueLine, isCutsceneUI));

            //save mouse position
            //Vector2 mousePos = Input.mousePosition; (moved to typing coroutine)
            
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
            
            //clear old choice button to active buttons array 
            _activeChoiceButtons.Clear();

            // enable and set info for buttons depending on ink choice information (choices (indexes) are going from up to down, but button indexes are revers)
            int choiceButtonIndex = dialogueChoices.Count - 1;
            for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
            {
                Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
                DialogueChoiceButton choiceButton = choiceButtons[choiceButtonIndex];

                //add new choice button to active buttons array
                _activeChoiceButtons.Add(choiceButton);
                //choiceButton.gameObject.SetActive(true); (will be activated in typing coroutine)
                
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
            //StartCoroutine(RestoreSelectionAfterFrame(mousePos)); (moved to typing coroutine)
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
            dialogueTextCutscene.text = "";
        }

        //tag events methods
        private void ChangeSpeaker1Name(string speakerNameTag)
        {
            //Debug.Log("ChangeSpeaker1Name: " + speakerNameTag);
            speaker1Text.text = char.ToUpper(speakerNameTag[0]) + speakerNameTag.Substring(1);
        }
        
        private void ChangeSpeaker2Name(string speakerNameTag)
        {
            //Debug.Log("ChangeSpeaker2Name: " + speakerNameTag);
            speaker2Text.text = char.ToUpper(speakerNameTag[0]) + speakerNameTag.Substring(1);
        }
        
        
        private void ChangePortrait1(string portraitNameTag)
        {
            //Debug.Log("ChangePortrait1: " + portraitNameTag);
            portrait1Image.sprite = GetPortraitSprite(portraitNameTag);
            
        }
        private void ChangePortrait2(string portraitNameTag)
        {
            //Debug.Log("ChangePortrait2: " + portraitNameTag);
            portrait2Image.sprite = GetPortraitSprite(portraitNameTag);
        }

        private Sprite GetPortraitSprite(string portraitNameTag)
        {
            string[] portraitNameParts = portraitNameTag.Split('_');
            foreach (var charAndDictPair in _characterPortraits) //try to find dictionary with sprites for current character
            {
                if (charAndDictPair.Key == portraitNameParts[0])
                {
                    //Debug.Log("we found dictionary with name "+charAndDictPair.Key);
                    foreach (var portraitNameAndSpritePair in _characterPortraits[charAndDictPair.Key]) //try to find sprite with current emotion
                    {
                        if (portraitNameAndSpritePair.Key == portraitNameTag)
                        {
                            //Debug.Log("we found sprite with name "+portraitNameAndSpritePair.Key);
                            return portraitNameAndSpritePair.Value;
                        }
                    }
                    
                }
            }
            
            return null;
        }
        
        
        private void ChangeCurrentSpeaker(string currentSpeakerTag)
        {
            //Debug.Log("TagCurrentSpeaker: " + currentSpeakerTag);
            if (currentSpeakerTag.Equals("speaker1"))
            {
                speaker1Text.fontSize = 22;
                speaker1Text.fontStyle = FontStyles.Bold;
                speaker1Text.color = Color.white;
                portrait1Image.color = Color.white;
                
                
                speaker2Text.fontSize = 20;
                speaker2Text.fontStyle = FontStyles.Normal;
                speaker2Text.color = Color.grey;
                portrait2Image.color = Color.white;
            }
            else
            {
                speaker1Text.fontSize = 20;
                speaker1Text.fontStyle = FontStyles.Normal;
                speaker1Text.color = Color.grey;
                portrait1Image.color = Color.grey;
                
                
                speaker2Text.fontSize = 22;
                speaker2Text.fontStyle = FontStyles.Bold;
                speaker2Text.color = Color.white;
                portrait2Image.color = Color.white;
            }
        }

        private void DisableBackground()
        {
            background.gameObject.SetActive(false);
        }
        private void EnableBackground()
        {
            background.gameObject.SetActive(true);
        }
    }
}