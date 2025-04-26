using System;
using System.Collections.Generic;
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
        [SerializeField] private TextAsset[] inkJson;

        private Dictionary<string, Story> _storiesMap = new Dictionary<string, Story>(); // all initialized stories (from json's)
        private Dictionary<Story, string> _storyToName = new Dictionary<Story, string>();//reverse dictionary (because we can't get name from story object)

        private Dictionary<string, InkDialogueVariables> _variablesMap = new Dictionary<string, InkDialogueVariables>();  //<storyName,variables>
        
        private Story _storyCurrent;

        private InkDialogueVariables _inkDialogueVariablesCurrent;
        
        private int _currentChoiceIndex = -1;

        private bool _dialoguePlaying = false;

        private InkExternalFunctions _inkExternalFunctions;

        private bool _cutsceneMode; //using this to set cutscene parameter in methods (almost for ui) //todo don't like a lot this cutscene realisation throw all system (think need to separate it)
        
        //tag system
        private const string Speaker1NameTag = "speaker1name";
        private const string PortraitTag1 = "portrait1";
        private const string Speaker2NameTag = "speaker2name";
        private const string PortraitTag2 = "portrait2";
        private const string CurrentSpeakerTag = "currentSpeaker";
        

        private void Awake()
        {

            
            if (Instance == null)
            {
                InitializeStoriesAndVariables();
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //get all compiled (json) stories (main) and add it into story map. Get variables and add it into variables map (with story.name key). Bind all ext functions for each story
        private void InitializeStoriesAndVariables()
        {
            _inkExternalFunctions = new InkExternalFunctions();
            foreach (var ink in inkJson)
            {
                //use json file as a name of story
                string key = ink.name;

                //create new object Story from TextAsset
                Story story = new Story(ink.text); // Инициализация Story из текста

                //add to story dictionary
                _storiesMap[key] = story;
                //add to revers dictionary
                _storyToName[story] = key;

                // Init InkDialogueVariables for iterable
                InkDialogueVariables dialogueVariables = new InkDialogueVariables(story);

                // add variables to dictionary <storyName,variables>
                _variablesMap[key] = dialogueVariables;
                    
                //bind all external variables to story (each)
                _inkExternalFunctions.Bind(story);
                    
                DialogDebug.Instance.LogFormat("Story key -  {0}| Story value - {1}|Variable key -  {2}, Variable value - {3}", key, story, _variablesMap.Keys, _variablesMap.Values);
            }
        }

        private void OnDestroy()
        {
            if (_storiesMap != null)
            {
                foreach (var storyPair in _storiesMap)
                {
                    _inkExternalFunctions.Unbind(storyPair.Value);
                }
            }
        }

        private void OnEnable()
        {
            EventManager.Instance.DialogueEvents.OnEnterDialogue += EnterDialogue;
            EventManager.Instance.InputEvents.OnSubmitPressed += SubmitPressed;
            EventManager.Instance.DialogueEvents.OnUpdateChoiceIndex += UpdateChoiceIndex;
            EventManager.Instance.DialogueEvents.OnUpdateInkDialogueVariable += UpdateVariableValueForStory;
            EventManager.Instance.QuestEvents.OnQuestStateChange += QuestStateChangeForDialogue;
        }

        private void OnDisable()
        {
            EventManager.Instance.DialogueEvents.OnEnterDialogue -= EnterDialogue;
            EventManager.Instance.InputEvents.OnSubmitPressed -= SubmitPressed;
            EventManager.Instance.DialogueEvents.OnUpdateChoiceIndex -= UpdateChoiceIndex;
            EventManager.Instance.DialogueEvents.OnUpdateInkDialogueVariable -= UpdateVariableValueForStory;
            EventManager.Instance.QuestEvents.OnQuestStateChange -= QuestStateChangeForDialogue;
        }

        //scripts can update variables map (updates quest state variables)
        //listens quest state changes, finds in variables key that contains quest name, get value and update variable value in appropriate story
        private void QuestStateChangeForDialogue(Quest quest)
        {
            //if (quest.InfoSo.Id + "State" == "SecondDevQuestState")
            //{
            //    Debug.Log("SecondDevQuestState");
            //    Debug.Log(quest.InfoSo.Id+" quest state changed to " + quest.StateEnum);
            //}
            
            foreach (var pair in _variablesMap)
            {
                if (pair.Value.GetVariables().ContainsKey(quest.InfoSo.Id + "State"))
                {
                    UpdateVariableValueForStory(pair.Key,quest.InfoSo.Id + "State", new StringValue(quest.StateEnum.ToString()) );
                }
                
            }
            
            //EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable_old(
            //quest.InfoSo.Id + "State",
            //new StringValue(quest.StateEnum.ToString()));
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
            
            ContinueOrExitStory(_cutsceneMode);
        }
        

        private void EnterDialogue(string storyName, string knotName, bool isCutsceneUI)
        {
            _cutsceneMode = isCutsceneUI;
            
            //don't start dialogue if already started
            if (_dialoguePlaying)
            {
                return;
            }
            
            /*
            //---------------------------test---------------------------------------

            if (_storyCurrent == null)
            {
                Debug.Log("Story is null");
                //initialize actual story variables from dialogue component before start dialogue
                _storyCurrent = new Story(inkJson.text);

                _inkExternalFunctions = new InkExternalFunctions();
                _inkExternalFunctions.Bind(_storyCurrent);

                _inkDialogueVariablesCurrent = new InkDialogueVariables(_storyCurrent);
                //-------------
            }

            //Debug.Log("first check = "+ _storyCurrent.variablesState.GetVariableWithName("FirstDialogueFirstNpc_1_main_var"));
            //_storyCurrent.variablesState["FirstDialogueFirstNpc_1_main_var"] = "new state";
            //Debug.Log("second check = "+ _storyCurrent.variablesState.GetVariableWithName("FirstDialogueFirstNpc_1_main_var"));
            if(!_storyCurrent.variablesState["FirstDialogueFirstNpc_1_main_var"].Equals(1)){
                Debug.Log("checking saving variable value in story runtime");
                UpdateInkDialogueVariable_old("FirstDialogueFirstNpc_1_main_var", new StringValue("1"));
            }
            else
            {
                Debug.Log("value was updated previously");
                Debug.Log("stored value is  =" + _storyCurrent.variablesState["FirstDialogueFirstNpc_1_main_var"]);
            }


            //--------------------------------------------------------------------
            */
            
            
            //UpdateVariableValueForStory("FirstDialogueNpc_main", "FirstDialogueFirstNpc_1_main_var", new StringValue("1"));
            
            _dialoguePlaying = true;
            
            //inform other parts of system that  we've started dialogue
            if (_cutsceneMode)
            {
                EventManager.Instance.DialogueEvents.DialogueStartedCutscene();
            }
            else
            {
                EventManager.Instance.DialogueEvents.DialogueStarted();
            }

            //input event context (change input context when starting dialogue)
            EventManager.Instance.InputEvents.ChangeInputEventContext(InputEventContext.Dialogue);
            
            //disable hotkeys (menus) during dialogue
            EventManager.Instance.InputEvents.HotkeysAreActive(false);
            
            //assigning current story variable (depending on provided param from dialogue starter)
            _storyCurrent = _storiesMap[storyName];
            DialogDebug.Instance.Log("Current story name - " + _storyToName[_storyCurrent]);

            //jump to the knot
            if (!knotName.Equals(""))
            {
                _storyCurrent.ChoosePathString(knotName);
            }
            else
            {
                DialogDebug.Instance.LogWarning("Knot name is empty when entering dialogue");
            }
            
            //update variables for current story and start listening for variables change (if implemented var change by ink inner logic)
            _inkDialogueVariablesCurrent = _variablesMap[storyName];
            _inkDialogueVariablesCurrent.SyncVariablesAndStartListening(_storyCurrent);
            
            //kick off the story
            ContinueOrExitStory(_cutsceneMode);
        }

        private void UpdateVariableValueForStory(string storyName, string variableName, Ink.Runtime.Object value)
        {
            if (_storiesMap.TryGetValue(storyName, out Story targetStory))
            {
                _variablesMap[storyName].UpdateVariableState(variableName, value);
                _variablesMap[storyName].SyncVariablesToStory(targetStory);
            }
            else
            {
                DialogDebug.Instance.LogWarning($"Story with name {storyName} not found.");
            }
        }

        private void ContinueOrExitStory(bool isCutsceneUI)
        {
            //make a choice, if applicable
            if (_storyCurrent.currentChoices.Count > 0 && _currentChoiceIndex != -1)
            {
                _storyCurrent.ChooseChoiceIndex(_currentChoiceIndex);
                //reset choice index for next time
                _currentChoiceIndex = -1;
            }

            if (_storyCurrent.canContinue)
            {
                string dialogueLine = _storyCurrent.Continue();
                
                //handle tags
                HandleTags(_storyCurrent.currentTags);

                // handle the case where there's an empty line of dialogue
                // by continuing until we get a line with content
                while (IsLineBlank(dialogueLine) && _storyCurrent.canContinue)
                {
                    dialogueLine = _storyCurrent.Continue();
                }

                // handle the case where the last line of dialogue is blank
                // (empty choice, external function, etc...)
                if (IsLineBlank(dialogueLine) && !_storyCurrent.canContinue)
                {
                    ExitDialogue();
                }
                else
                {
                    EventManager.Instance.DialogueEvents.DisplayDialogue(dialogueLine, _storyCurrent.currentChoices,_cutsceneMode);
                }
            }
            else if (_storyCurrent.currentChoices.Count == 0)
            {
                ExitDialogue();
            }
        }

        private void HandleTags(List<string> currentTags)
        {
            // Loop for each tag in line and handle it accordingly
            foreach (var tag in currentTags)
            {
                // parse the tag 
                string[] splitTag = tag.Split(':');
                if (splitTag.Length != 2)
                {
                    DialogDebug.Instance.LogError($"Tag could not be appropriately parsed: {tag}");
                }
                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();
                
                // handle the tag
                switch (tagKey)
                {
                    case Speaker1NameTag:
                        //DialogDebug.Instance.Log("Speaker tag 1=" + tagValue);
                        EventManager.Instance.DialogueEvents.TagChangeSpeaker1Name(tagValue);
                        break;
                    case PortraitTag1:
                        //DialogDebug.Instance.Log("Portrait tag 1=" + tagValue);
                        EventManager.Instance.DialogueEvents.TagChangePortrait1(tagValue);
                        break;
                    case Speaker2NameTag:
                        //DialogDebug.Instance.Log("Speaker tag 2=" + tagValue);
                        EventManager.Instance.DialogueEvents.TagChangeSpeaker2Name(tagValue);
                        break;
                    case PortraitTag2:
                        //DialogDebug.Instance.Log("Portrait tag 2=" + tagValue);
                        EventManager.Instance.DialogueEvents.TagChangePortrait2(tagValue);
                        break;
                    case CurrentSpeakerTag:
                        EventManager.Instance.DialogueEvents.TagChangeCurrentSpeaker(tagValue);
                        break;
                    default:
                        DialogDebug.Instance.LogWarning($"Tag could not currently being handled: {tag}");
                        break;
                }
            }
        }

        private void ExitDialogue()
        {
            DialogDebug.Instance.Log("Exit dialogue for story - " + _storyToName[_storyCurrent]);
            
            _dialoguePlaying = false;
            //inform other parts of system that  we've finished dialogue
            EventManager.Instance.DialogueEvents.DialogueFinished();
            EventManager.Instance.DialogueEvents.DialogueFinishedCutscene();
            
            //input event context (change input context back to default when dialogue ends)
            EventManager.Instance.InputEvents.ChangeInputEventContext(InputEventContext.Default);
            
            //enable hotkeys (menus) during dialogue
            EventManager.Instance.InputEvents.HotkeysAreActive(true);
            
            //stop listening for variables
            _inkDialogueVariablesCurrent.StopListening(_storyCurrent);
            
            //reset story state
            _storyCurrent.ResetState();
        }

        private bool IsLineBlank(string dialogueLine)
        {
            return dialogueLine.Trim().Equals("")|| dialogueLine.Trim().Equals("\n");
        }
    }
}