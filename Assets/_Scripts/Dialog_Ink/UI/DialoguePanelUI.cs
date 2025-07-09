using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Managers;
using Ink.Runtime;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.Dialog_Ink.UI
{
    public class DialoguePanelUI : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private float typingSpeed = 0.04f;
        private Coroutine _typingCoroutine;
        private bool _isDialogueActive;
        private bool _isSubmitPressed;
        private bool _isSkipPressed;


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
        
        [Header("Player portraits")]
        [SerializeField] private List<Sprite> playerPortraitsSpriteList = new ();
        
        [Header("Mother portraits")]
       
        [SerializeField] private List<Sprite> motherPortraitsSpriteList = new ();
        
        private Dictionary<string, Sprite> _characterPortraits =new (); //dictionary for all portraits
        

        private void Awake()
        {
          contentParent.SetActive(false);
          contentParentCutscene.SetActive(false);
          
          ResetText();
          ResetUiElements();
          
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
            EventManager.Instance.DialogueEvents.OnSkipTypingText += SkipPressed;
            
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
            EventManager.Instance.DialogueEvents.OnSkipTypingText -= SkipPressed;
            
            //tags
            EventManager.Instance.DialogueEvents.OnTagChangeSpeaker1Name -= ChangeSpeaker1Name;
            EventManager.Instance.DialogueEvents.OnTagChangeSpeaker2Name -= ChangeSpeaker2Name;
            EventManager.Instance.DialogueEvents.OnTagChangePortrait1 -= ChangePortrait1;
            EventManager.Instance.DialogueEvents.OnTagChangePortrait2 -= ChangePortrait2;
            EventManager.Instance.DialogueEvents.OnTagChangeCurrentSpeaker -= ChangeCurrentSpeaker;
        }
        
        //create dictionary of characters and their sprite portraits
        private void InitializePortraitsDictionary()
        {
            //Dictionary<string, Sprite> innerPlayerPortraitsDictionary = new Dictionary<string, Sprite>();
            foreach (var sprite in playerPortraitsSpriteList)
            {   
                _characterPortraits.Add(sprite.name, sprite);
            }
            
            //Dictionary<string, Sprite> innerNPCPortraitsDictionary = new Dictionary<string, Sprite>();
            foreach (var sprite in motherPortraitsSpriteList)
            {   
                _characterPortraits.Add(sprite.name, sprite);
            }
            
            //_characterPortraits = new Dictionary<string, Dictionary<string, Sprite>>();
            //_characterPortraits.Add("player", innerPlayerPortraitsDictionary);
            //_characterPortraits.Add("npc", innerNPCPortraitsDictionary);
        }
        
        
        private void DialogueStarted()
        {
            _isDialogueActive = true;
            contentParent.SetActive(true);
        }
        
        private void DialogueFinished()
        {
            _isDialogueActive = false;
            contentParent.SetActive(false);
            
            //reset anything for nex time
            ResetText();
            
            //reset ui
            ResetUiElements();
        }
        private void DialogueStartedCutscene()
        {
            _isDialogueActive = true;
            contentParentCutscene.SetActive(true);
        }
        
        private void DialogueFinishedCutscene()
        {
            _isDialogueActive = false;
            contentParentCutscene.SetActive(false);
            
            //reset anything for nex time
            ResetText();
        }

        private void HideActiveButtons()
        {
            //EventSystem.current.SetSelectedGameObject(null);
            
            //Debug.Log(_activeChoiceButtons!=null);
            //Debug.Log(_activeChoiceButtons.Count);
            if (_activeChoiceButtons!=null && _activeChoiceButtons.Count != 0)
            {
                foreach (var buttonScript in _activeChoiceButtons)
                {
                    //buttonScript.GetComponent<Button>().OnPointerExit(new PointerEventData(EventSystem.current));
                    //buttonScript.GetComponent<Button>().GetComponent<Selectable>().OnDeselect(null);
                    buttonScript.gameObject.SetActive(false);
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
                EventManager.Instance.DialogueEvents.ChoiceButtonsAppears();
            }
        }

        private void SubmitPressed(InputEventContext context)
        {
            if (context == InputEventContext.TypingLine)
            {
                _isSubmitPressed = true;
            }
        }
        private void SkipPressed() //used for skip typing animation
        {
            //SubmitPressed(EventManager.Instance.InputEvents.GetInputEventContext());
            StartCoroutine(SkipCoroutine());
        }

        private IEnumerator SkipCoroutine()
        {
            yield return null;
            _isSkipPressed = true;
        }
        

        //displays alphabetically (char to char)  | old realization
        /*private IEnumerator DisplayTypingLine(string line, bool isCutscene)
        {
            
            //hide continue icon
            continueIcon.SetActive(false);
            
            //set context ty "typing" to prevent line skipping
            EventManager.Instance.InputEvents.SetInputEventContext(InputEventContext.TypingLine);
            
            
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
            
            //Debug.Log("before typing <-----------------------------------");
            
            //typing char by char
            foreach (var letter in line.ToCharArray())
            {
                //skip typing "animation" if submit pressed
                if (_isSubmitPressed)
                {
                    localTextVar.text = line;
                    break;
                }

                if (_isSkipPressed)
                {
                    //Debug.Log("skip typing <-----------------------------------");
                    localTextVar.text = line;
                    _isSkipPressed = false;
                    break;
                }

                if (!_isDialogueActive)
                {
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
            _isSubmitPressed = false;
            
            //restore context if dialogue active
            if (_isDialogueActive)
            {
                EventManager.Instance.InputEvents.SetInputEventContext(InputEventContext.Dialogue);
            }
            
            //show buttons after typing
            ShowActiveButtons();
            
            //show continue icon
            continueIcon.SetActive(true);
            
            //restore highlight down the cursor
            //StartCoroutine(RestoreSelectionAfterFrame(mousePos));
        }*/
        
        //new realization by typing char to char via "maxVisibleCharacters()"
        private IEnumerator DisplayTypingLine(string line, bool isCutscene)
        {

            //hide continue icon
            continueIcon.SetActive(false);

            //set context ty "typing" to prevent line skipping
            EventManager.Instance.InputEvents.SetInputEventContext(InputEventContext.TypingLine);


            TMP_Text localTextVar;
            if (isCutscene)
            {
                localTextVar = dialogueTextCutscene;
            }
            else
            {
                localTextVar = dialogueText;
            }

            //make text invisible and update mesh
            localTextVar.alpha = 0f; //additional insurance (we can use "maxVisibleCharacters=0" after "localTextVar.text = line;" instead)
            localTextVar.text = line;
            localTextVar.ForceMeshUpdate();
            

            //Debug.Log("before typing <-----------------------------------");

            //typing char by char
            for(var i = 0; i < line.Length; i++)
            {
                //skip typing "animation" if submit pressed
                if (_isSubmitPressed)
                {
                    localTextVar.alpha = 1f;
                    localTextVar.maxVisibleCharacters = line.Length;
                    break;
                }

                if (_isSkipPressed)
                {
                    //Debug.Log("skip typing <-----------------------------------");
                    localTextVar.alpha = 1f;
                    localTextVar.maxVisibleCharacters = line.Length;
                    _isSkipPressed = false;
                    break;
                }

                if (!_isDialogueActive)
                {
                    break;
                }

                /*
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
                {*/
                    localTextVar.alpha = 1f;
                    localTextVar.maxVisibleCharacters = i+1;
                    yield return new WaitForSeconds(typingSpeed);
                //}

            }

            //reset submit state
            _isSubmitPressed = false;

            //restore context if dialogue active
            if (_isDialogueActive)
            {
                EventManager.Instance.InputEvents.SetInputEventContext(InputEventContext.Dialogue);
            }

            //show buttons after typing
            ShowActiveButtons();

            //show continue icon
            continueIcon.SetActive(true);

            //restore highlight down the cursor
            //StartCoroutine(RestoreSelectionAfterFrame(mousePos));
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
            HideActiveButtons();
            
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

        //the method is outdated, as it was used for instant text printing. With typing animation there is enough time to clear selection\highlight 
        private IEnumerator RestoreSelectionAfterFrame(Vector2 mousePosition)        {
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
    
            // reset select state (for all elements?)
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void ResetText()
        {
            dialogueText.text = "";
            dialogueTextCutscene.text = "";
        } 
        private void ResetUiElements()
        {
            speaker1Text.text = null;
            speaker2Text.text = null;
            portrait1Image.sprite = null;
            portrait2Image.sprite = null;
            
            portrait1Image.color = Color.white;
            portrait2Image.color = Color.white;
            
            setFirstSpeakerSpriteActive(false);
            setSecondSpeakerSpriteActive(false);
        }

        //tag events methods //todo handle null - add default stub 
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
            setFirstSpeakerSpriteActive(true);
            portrait1Image.sprite = GetPortraitSprite(portraitNameTag);
        }

        private void ChangePortrait2(string portraitNameTag)
        {
            setSecondSpeakerSpriteActive(true);
            portrait2Image.sprite = GetPortraitSprite(portraitNameTag);
        }

        /*private Sprite GetPortraitSprite(string portraitNameTag)
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
        }*/

        [NotNull]
        private Sprite GetPortraitSprite(string portraitNameTag)
        {
            var portrait = _characterPortraits[portraitNameTag];

            return portrait;
        }


        private void ChangeCurrentSpeaker(string currentSpeakerTag)
        {
            //Debug.Log("TagCurrentSpeaker: " + currentSpeakerTag);
            if (currentSpeakerTag.Equals("speaker1"))
            {
                //speaker1Text.fontSize = 22;
                //speaker1Text.fontStyle = FontStyles.Bold;
                //speaker1Text.color = Color.white;
                portrait1Image.color = Color.white;
                
                
                //speaker2Text.fontSize = 20;
                //speaker2Text.fontStyle = FontStyles.Normal;
                //speaker2Text.color = Color.grey;
                portrait2Image.color = Color.grey;
            }
            else
            {
                //speaker1Text.fontSize = 20;
                //speaker1Text.fontStyle = FontStyles.Normal;
                //speaker1Text.color = Color.grey;
                portrait1Image.color = Color.grey;
                
                
                //speaker2Text.fontSize = 22;
                //speaker2Text.fontStyle = FontStyles.Bold;
                //speaker2Text.color = Color.white;
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

        private void setFirstSpeakerSpriteActive(bool active)
        {
            portrait1Image.gameObject.SetActive(active);
        }
        private void setSecondSpeakerSpriteActive(bool active)
        {
            portrait2Image.gameObject.SetActive(active);
        }
    }
}