using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Characters.NPC;
using _Scripts.Enums;
using _Scripts.Managers;
using _Scripts.Service.Log;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace _Scripts.Dialog_Ink
{
    /// <summary>
    /// This script stores dialogue knots for current character. Also knows main story name.
    /// Uses as dialogue start point. Get actual dialogue Knot / actual story (can be changed in future) and dialogue variables and send it (start dialogue) via event in manager.
    /// Uses with Game Object with NpcCharAbstract child and QuestPoint(not necessary) scripts 
    /// </summary>
    public class StandaloneDialogueComponent : MonoBehaviour
    {
        //+add story(ies) array. Can try with one story and multiple knots.
        //+start dialogue method need to share story file (which will be initialized (bind story and vars) and then start dialogue from actual knot
        
        //+array of knots ? (list of knot objects and dictionary for is complete)
        //+if there is npc character component (to add) get name from there
        //npc need to have char component, questPoint(optional), dialogue component. they can share each other info.
        
        //+todo !! first we need to try change in game dialogue variable, store it in this script and then Dia Manager must update it from here
        //+need to handle external universal external functions

        //+since we will update current (from this script) variables for current (from this script) story, it must not be a problem
        //-do we need an abstract class and custom children ?
        
        //+todo second we need to complete start dialogue and then continue with second dialogue (knot ?)
        
        //-todo third we need to add char script with lists/dictionaries to store stories/knots and methods (get) to share actual one
        

        //+-TODO INTEGRATE QUESTS (questpoint) -> need to handle start\finish dialogue (by using variables in char method ->
        //-TODO -> quest point must trigger it using list of quests/ manager, but is there no quests (variables) in dialogue?). mb quest point must handle self quests and change variables in dialogue ?
        
        //+TODO add npc emotes
        //TODO HANDLE NPC APPEAR IN CURRENT PLACE (like trigger spawner) (add places/locations fileds, on/off for model(sprite/collider) but script go is on allways to track game states? npc manager with list of npc's (prefabs) ? methods to move to locations (some logic?)? sprites for emotions etc..
        
        
        [Header("Current Actual Dialogues")]
        [SerializeField] protected string currentActualStoryName;
        [SerializeField] protected string currentActualDialogueKnotName;
        [SerializeField] protected string currentCustomDialogueKnotName;

        [Header("Ink Include Files (for names only)")]
        [SerializeField] private List<string> dialogueKnotNames; //was protected List<Object> inkIncludeFiles - and this fng engine deletes this objects in build... replaced by strings
        [SerializeField] private List<string> customDialogueKnotNames;

        private Dictionary<string, bool> _dialogueKnotStates = new Dictionary<string, bool>(); // "dialogueKnotName" (string) + "isComplete" (bool) (for current dialogue knot)
        private Dictionary<string, bool> _customDialogueKnotStates = new Dictionary<string, bool>();
        
        [Header("Misc")]
        [SerializeField]private string currentNpcName;
        [SerializeField]private bool isDialogueAutoActivationEnabled;

        protected virtual void Start() //change to start cause of dialogueDebug sometimes initialized after this (in awake) //todo add lazy initialization to dialogueDibug ?
        {
            InitializeDialogueStates();
            currentNpcName = GetComponent<NpcCharAbstract>().GetNpcName();
            SetNextCurrentDialogueKnot();
        }
        
       
        private void OnEnable()
        {
            EventManager.Instance.DialogueEvents.OnCompleteDialogueKnot += CompleteDialogueKnotInDictionary;
            EventManager.Instance.DialogueEvents.OnSetDialogueAutoActivation += ToggleDialogueAutoActivation;
            EventManager.Instance.DialogueEvents.OnUpdateDialogueStatesFromUI += UpdateDialogueStatesFromUI;
            EventManager.Instance.DialogueEvents.OnSetCustomDialogueKnot += SetCustomDialogueKnot;
            

        }
        private void OnDisable()
        {
            EventManager.Instance.DialogueEvents.OnCompleteDialogueKnot -= CompleteDialogueKnotInDictionary;
            EventManager.Instance.DialogueEvents.OnSetDialogueAutoActivation -= ToggleDialogueAutoActivation;
            EventManager.Instance.DialogueEvents.OnUpdateDialogueStatesFromUI -= UpdateDialogueStatesFromUI;
            EventManager.Instance.DialogueEvents.OnSetCustomDialogueKnot += SetCustomDialogueKnot;
        }

        //add list of knot names to dictionary

        private void InitializeDialogueStates()
        {
            //default dialogue knots
            _dialogueKnotStates = new Dictionary<string, bool>();
            
            foreach (var knotName in dialogueKnotNames)
            {
                if (!string.IsNullOrEmpty(knotName))
                {
                    _dialogueKnotStates[knotName] = false;
                }
            }
    
            if (_dialogueKnotStates.Count == 0)
            {
                Debug.LogError($"No dialogue knots configured for {gameObject.name}");
            }
            
            //custom dialogue knots
            _customDialogueKnotStates = new Dictionary<string, bool>();
            
            foreach (var knotName in customDialogueKnotNames)
            {
                if (!string.IsNullOrEmpty(knotName))
                {
                    _customDialogueKnotStates[knotName] = false;
                }
            }
            
            if (_customDialogueKnotStates.Count == 0)
            {
                DialogDebug.Instance.Log($"No custom dialogue knots for {currentNpcName} character or entity");
            }
        }

        /// /// <summary>
        /// Starts dialogue with input protection:
        /// 1. Locks input to prevent conflicting submissions (in ClickDialogueActivatorForNpc)
        /// 2. Delays initialization by 1 frame (in StartDialogue())
        /// 3. Processes inputs safely in dialogue context
        /// 
        /// Sequence:
        /// Click → [SetSubmitLock] → Frame1(InputBlocked)
        ///           ↓
        /// Frame2(Unlock+Start) → Frame3(CleanInput)
        /// </summary>
        public void StartDialogue(bool isCutsceneUI)
        {
            if (!String.IsNullOrEmpty(currentCustomDialogueKnotName))
            {
                //Debug.Log("Starting custom dialogue 3333333333333");
                StartCoroutine(StartCustomDialogueDelayed());
            }
            else
            {
                // start after one frame (we handle mouse button situation after dialogue started)
                StartCoroutine(StartDialogueDelayed(isCutsceneUI));
            }
        }

        private IEnumerator StartDialogueDelayed(bool isCutsceneUI)
        {
            //Apply language setting
            string storyNameTemp = currentActualStoryName +"_"+ LocalizationManager.Instance.GetCurrentLanguageCode();
            
            // wait for next frame
            yield return null;
            
            
            EventManager.Instance.DialogueEvents.EnterDialogue(storyNameTemp, currentActualDialogueKnotName, isCutsceneUI);   
        }

        //custom dialogues.

        //have no queue. launches in priority if custom knot variable are not empty

        private IEnumerator StartCustomDialogueDelayed()
        {
            if (_customDialogueKnotStates.Count == 0)
            {
                Debug.LogError($"No custom dialogue knots configured for {gameObject.name}");
                yield break;
            }
            
            //Apply language setting
            string storyNameTemp = currentActualStoryName +"_"+ LocalizationManager.Instance.GetCurrentLanguageCode();
            
            // wait for next frame
            yield return null;
            
            //Debug.Log(currentActualDialogueKnotName);
            
            EventManager.Instance.DialogueEvents.EnterDialogue(storyNameTemp, currentCustomDialogueKnotName, false);   
        }


        //todo сделать пропуск времени кастомными диалогами (отдельный лист и словарь) сделать так же методы завершения кастомного диалога, но без зарядки следующего. Использовать курент кастом диалог переменную.

        //todo заряжать туда вручную Нужен ли словарь ? мб просто лист ? или всё таки отыгрыш если надо обозначать, если вдруг понадобится инфа об отыгранном ?

        //todo add custom dialogues dictionary (and list in the component) and integrate it in system ? we can start it with priority in som circumstances (with high priority on click). Need new events and variables to save custom start dialogue state on click

        //invokes subscribed method for this GO if we match the name for this character. Also check the knot name variable. 

        private void CompleteDialogueKnotInDictionary(string characterName, string knotName)
        {
            DialogDebug.Instance.Log("Invoked CompleteDialogueKnotInDictionary method for \"" + gameObject.name +
                                     "\" game object");
            if (characterName != currentNpcName)
            {
                DialogDebug.Instance.Log("Comparison of names in CompleteDialogueKnotInDictionary method for \""+gameObject.name+"\" game object led to return from method");
                DialogDebug.Instance.Log("Name from external function is \""+characterName+ "\"");
                DialogDebug.Instance.Log("Name from script is \""+currentNpcName+ "\"");
                return;
            }

            if (_dialogueKnotStates.ContainsKey(knotName))
            {
                DialogDebug.Instance.Log("Complete \"" + knotName +"\" for \""+gameObject.name+"\" game object");
                _dialogueKnotStates[knotName] = true;
                SetNextCurrentDialogueKnot();
            }
            else
            {
                DialogDebug.Instance.LogWarning("There is no dialogueKnotName \""+ knotName+"\" for \""+gameObject.name+"\" game object");
            }
        }


        //check for false "isComplete" flag for knot and set this knot as actual |knot order (must be 1,2,3..)

        private void SetNextCurrentDialogueKnot()
        {
            if (_dialogueKnotStates.Any(pair => !pair.Value))
            {
                var next = _dialogueKnotStates.First(pair => !pair.Value);
                currentActualDialogueKnotName = next.Key;
                DialogDebug.Instance.Log("Current Dialogue Knot \"" + currentActualDialogueKnotName + "\" is set for \"" + currentNpcName + "\" character");
            }
            else
            {
                DialogDebug.Instance.Log("All dialogue knots are completed for \"" + currentNpcName + "\" character.");
                // add new story name?
            }
        }

        private void SetCustomDialogueKnot(string npcName, string customDialogueKnot)
        {
            if (currentNpcName == npcName)
            {
                currentCustomDialogueKnotName = customDialogueKnot;
            }
        }

        //npc manager checks it and starts dialogue if this = true
        private void ToggleDialogueAutoActivation(string npcName, bool isActive)
        {
            if (currentNpcName.Equals(npcName))
            {
                isDialogueAutoActivationEnabled = isActive;
            }
        }
        
        public bool IsAutoActivationEnabled()
        {
            return isDialogueAutoActivationEnabled;
        }

        public Dictionary<string, bool> GetDialogueKnotStates()
        {
            return _dialogueKnotStates;
        }

        private void UpdateDialogueStatesFromUI(string npcName, string dialogName, bool isChecked )
        {
            if (npcName != currentNpcName)
            {
                return;
            }
            
            _dialogueKnotStates[dialogName] = isChecked;
            SetNextCurrentDialogueKnot();
        }
    }
}