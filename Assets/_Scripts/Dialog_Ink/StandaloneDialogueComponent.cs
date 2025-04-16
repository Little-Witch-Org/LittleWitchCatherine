using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Characters.NPC;
using _Scripts.Enums;
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
        
        
        [Header("Current Actual Dialogues)")]
        [SerializeField] protected string currentActualStoryName;
        [SerializeField] protected string currentActualDialogueKnotName;

        [Header("Ink Include Files (for names only)")]
        [SerializeField] private List<string> dialogueKnotNames; //was protected List<Object> inkIncludeFiles - and this fng engine deletes this objects in build... replaced by strings

        private Dictionary<string, bool> _dialogueKnotStates = new Dictionary<string, bool>(); // "dialogueKnotName" (string) + "isComplete" (bool) (for current dialogue knot)

        [SerializeField]private string currentCharacterName;

        protected virtual void Awake()
        {
            InitializeDialogueStates();
            currentCharacterName = GetComponent<NpcCharAbstract>().GetNpcName();
            SetNextCurrentDialogueKnot();
        }
        
       
        private void OnEnable()
        {
            EventManager.Instance.DialogueEvents.OnCompleteDialogueKnot += CompleteDialogueKnotInDictionary;
        }
        private void OnDisable()
        {
            EventManager.Instance.DialogueEvents.OnCompleteDialogueKnot -= CompleteDialogueKnotInDictionary;
        }

        //add list of knot names to dictionary
        private void InitializeDialogueStates()
        {
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
                Debug.LogError("No dialogue knots configured!");
            }
        }
        
        /// /// <summary>
        /// Starts dialogue with input protection:
        /// 1. Locks input to prevent conflicting submissions (in ClickTriggerActivatorForNpc)
        /// 2. Delays initialization by 1 frame (in StartDialogue())
        /// 3. Processes inputs safely in dialogue context
        /// 
        /// Sequence:
        /// Click → [LockInput] → Frame1(InputBlocked)
        ///           ↓
        /// Frame2(Unlock+Start) → Frame3(CleanInput)
        /// </summary>
        public void StartDialogue()
        {
            // start after one frame (we handle mouse button situation after dialogue started)
            StartCoroutine(StartDialogueDelayed());
        }

        private IEnumerator StartDialogueDelayed()
        {
            yield return null; // wait for next frame
            
            EventManager.Instance.DialogueEvents.EnterDialogue(currentActualStoryName, currentActualDialogueKnotName);
        }

        //invokes subscribed method for this GO if we match the name for this character. Also check the knot name variable. 
        private void CompleteDialogueKnotInDictionary(string characterName, string knotName)
        {
            DialogDebug.Instance.Log("Invoked CompleteDialogueKnotInDictionary method for \"" + gameObject.name +
                                     "\" game object");
            if (characterName != currentCharacterName)
            {
                DialogDebug.Instance.Log("Comparison of names in CompleteDialogueKnotInDictionary method for \""+gameObject.name+"\" game object led to return from method");
                DialogDebug.Instance.Log("Name from external function is \""+characterName+ "\"");
                DialogDebug.Instance.Log("Name from script is \""+currentCharacterName+ "\"");
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
                DialogDebug.Instance.Log("Current Dialogue Knot \"" + currentActualDialogueKnotName + "\" is set for \"" + currentCharacterName + "\" character");
            }
            else
            {
                DialogDebug.Instance.Log("All dialogue knots are completed for \"" + currentCharacterName + "\" character.");
                // add new story name?
            }
        }
    }
}