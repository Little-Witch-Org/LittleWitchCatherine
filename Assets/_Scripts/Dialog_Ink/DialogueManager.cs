using System;
using _Scripts.Enums;
using _Scripts.QuestSystem;
using _Scripts.Service.Log;
using Ink.Runtime;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        
        [Header("Ink Story")]
        [SerializeField] private TextAsset inkJson;
        
        private Story _story;

        private int _currentChoiceIndex = -1;
        
        private bool _dialoguePlaying = false;

        private InkExternalFunctions _inkExternalFunctions;
        
        private InkDialogueVariables _inkDialogueVariables;
        
        private void Awake()
        {

            
            if (Instance == null)
            {
                _story = new Story(inkJson.text);
                _inkExternalFunctions = new InkExternalFunctions();
                _inkExternalFunctions.Bind(_story);
                _inkDialogueVariables = new InkDialogueVariables(_story);
                
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            //handle singleton object (copy doesn't have bound story)
            if (_story != null)
            {
                _inkExternalFunctions.Unbind(_story);
            }
        }

        private void OnEnable()
        {
            EventManager.Instance.DialogueEvents.OnEnterDialogue += EnterDialogue;
            EventManager.Instance.InputEvents.OnSubmitPressed += SubmitPressed;
            EventManager.Instance.DialogueEvents.OnUpdateChoiceIndex += UpdateChoiceIndex;
            EventManager.Instance.DialogueEvents.OnUpdateInkDialogueVariable += UpdateInkDialogueVariable;
            EventManager.Instance.QuestEvents.OnQuestStateChange += QuestStateChange;
        }

        private void OnDisable()
        {
            EventManager.Instance.DialogueEvents.OnEnterDialogue -= EnterDialogue;
            EventManager.Instance.InputEvents.OnSubmitPressed -= SubmitPressed;
            EventManager.Instance.DialogueEvents.OnUpdateChoiceIndex -= UpdateChoiceIndex;
            EventManager.Instance.DialogueEvents.OnUpdateInkDialogueVariable -= UpdateInkDialogueVariable;
            EventManager.Instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
        }

        //scripts can update variables state (need to try it)
        private void QuestStateChange(Quest quest)
        {
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable(
            quest.InfoSo.Id + "State",
            new StringValue(quest.StateEnum.ToString()));
        }
        
        //manually update variable
        private void UpdateInkDialogueVariable(string varName, Ink.Runtime.Object value)
        {
            _inkDialogueVariables.UpdateVariableState(varName, value);
        }

        private void UpdateChoiceIndex(int choiceIndex)
        {
            _currentChoiceIndex = choiceIndex;
        }

        private void SubmitPressed(InputEventContext context)
        {
            //if contest isn't dialogue -> don't register button here
            if (!context.Equals(InputEventContext.Dialogue))
            {
                return;
            }
            
            ContinueOrExitStory();
        }


        private void EnterDialogue(string knotName)
        {
            //don't start dialogue if already started
            if (_dialoguePlaying)
            {
                return;
            }
            _dialoguePlaying = true;
            
            //inform other parts of system that  we've started dialogue
            EventManager.Instance.DialogueEvents.DialogueStarted();
            
            //input event context (change input context when starting dialogue)
            EventManager.Instance.InputEvents.ChangeInputEventContext(InputEventContext.Dialogue);
            

            //jump to the knot
            if (!knotName.Equals(""))
            {
                _story.ChoosePathString(knotName);
            }
            else
            {
                DialogDebug.Instance.LogWarning("Knot name is empty when entering dialogue");
            }
            
            //start listening for variables
            _inkDialogueVariables.SyncVariablesAndStartListening(_story);
            
            //kick off the story
            ContinueOrExitStory();
        }

        private void ContinueOrExitStory()
        {
            //make a choice, if applicable
            if (_story.currentChoices.Count > 0 && _currentChoiceIndex != -1)
            {
                _story.ChooseChoiceIndex(_currentChoiceIndex);
                //reset choice index for next time
                _currentChoiceIndex = -1;
            }

            if (_story.canContinue)
            {
                string dialogueLine = _story.Continue();

                // handle the case where there's an empty line of dialogue
                // by continuing until we get a line with content
                while (IsLineBlank(dialogueLine) && _story.canContinue)
                {
                    dialogueLine = _story.Continue();
                }

                // handle the case where the last line of dialogue is blank
                // (empty choice, external function, etc...)
                if (IsLineBlank(dialogueLine) && !_story.canContinue)
                {
                    ExitDialogue();
                }
                else
                {
                    EventManager.Instance.DialogueEvents.DisplayDialogue(dialogueLine, _story.currentChoices);
                }
            }
            else if (_story.currentChoices.Count == 0)
            {
                ExitDialogue();
            }
        }

        private void ExitDialogue()
        {
            DialogDebug.Instance.Log("Exit dialogue");
            
            _dialoguePlaying = false;
            //inform other parts of system that  we've finished dialogue
            EventManager.Instance.DialogueEvents.DialogueFinished();
            
            //input event context (change input context back to default when dialogue ends)
            EventManager.Instance.InputEvents.ChangeInputEventContext(InputEventContext.Default);
            
            //stop listening for variables
            _inkDialogueVariables.StopListening(_story);
            
            //reset story state
            _story.ResetState();
        }

        private bool IsLineBlank(string dialogueLine)
        {
            return dialogueLine.Trim().Equals("")|| dialogueLine.Trim().Equals("\n");
        }
    }
}