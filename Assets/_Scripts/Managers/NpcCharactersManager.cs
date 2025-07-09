using System;
using System.Collections.Generic;
using _Scripts.Characters.NPC;
using _Scripts.Dialog_Ink;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Managers
{
    /// <summary>
    /// Script contains all npc characters (for novel view now).
    /// Enables and disables npc (body) according to conditions (place/time etc) using events
    /// todo add place changer for npc (npc script must contain change place logic - time etc) appear/disappear from screen using events
    /// </summary>
    public class NpcCharactersManager : MonoBehaviour
    {
        public static NpcCharactersManager Instance;
        
        [SerializeField] private List<GameObject> npcCharacterPrefabs;
        [SerializeField] private GameObject npcCharactersContainer;
        private List<GameObject>  _npcCharacters = new List<GameObject>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializeManager();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            EventManager.Instance.TransitionEvents.OnCurrentPlaceOnScreen += CheckSpawnConditions;
            EventManager.Instance.DialogueEvents.OnStartDialogueWithNpc += StartDialogueWithCurrentNpc;
            
            EventManager.Instance.GameEvents.OnStoryModActivated += DisableTestNPCs;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.TransitionEvents.OnCurrentPlaceOnScreen -= CheckSpawnConditions;
            EventManager.Instance.DialogueEvents.OnStartDialogueWithNpc -= StartDialogueWithCurrentNpc;
            
            EventManager.Instance.GameEvents.OnStoryModActivated -= DisableTestNPCs;
        }
        

        //instantiate all npc characters
        private void InitializeManager()
        {
            foreach (var npcPrefab in npcCharacterPrefabs)
            {
                GameObject characterInstance = Instantiate(npcPrefab, npcCharactersContainer.transform);
                characterInstance.GetComponent<NpcCharAbstract>().DisableBody(); //disable body by default
                _npcCharacters.Add(characterInstance);
            }
        }


        private void CheckSpawnConditions(string location, string place)
        {
            
            //OnScreen - current location manager location (char might be in another place)
            var currentOnScreenLocation = TransitionManager.Instance.GetCurrentLocation();
            var currentOnScreenPlace = TransitionManager.Instance.GetCurrentPlace();

            
            //check all Npc's
            foreach (var npcObject in _npcCharacters)
            {
                var npcLocation = npcObject.GetComponent<NpcCharAbstract>().GetCurrentLocationName();
                var npcPlace = npcObject.GetComponent<NpcCharAbstract>().GetCurrentPlaceName();

                //Debug.Log(npcObject.GetComponent<NpcCharAbstract>().GetCurrentLocationName());
                //Debug.Log(npcObject.GetComponent<NpcCharAbstract>().GetCurrentPlaceName());
                
                //enable/disable npc according to scene state
                if (npcLocation.Equals(currentOnScreenLocation) &&
                    npcPlace.Equals(currentOnScreenPlace))
                {
                    
                    //Debug.LogFormat("Npc character \"{0}\"enabled",npcObject.GetComponent<NpcCharAbstract>().GetNpcName());
                    EnableNpc(npcObject);
                    
                    //activate dialogue if autoactivation is enabled
                    CheckAutostartDialogueAndActivate(npcObject);
                }
                else
                {
                    if (npcObject.GetComponent<NpcCharAbstract>().IsNpcActive())
                    {
                        //Debug.LogFormat("Npc character \"{0}\"disabled",npcObject.GetComponent<NpcCharAbstract>().GetNpcName());
                        DisableNpc(npcObject);
                    }
                }
            }
        }

        private void EnableNpc(GameObject npcObject)
        {
            npcObject.GetComponent<NpcCharAbstract>().EnableBody();
        }

        private void DisableNpc(GameObject npcObject)
        {
            npcObject.GetComponent<NpcCharAbstract>().DisableBody();
        }

        public void MoveNpc()
        {
            //moves npc according to conditions 
        }

        private void StartDialogueWithCurrentNpc(string characterName)
        {
            //Debug.Log(characterName);
            StandaloneDialogueComponent foundComponent = null;

            //Debug.Log("Trying to start dialogue with current character "+characterName);
            foreach (var charObj in _npcCharacters)
            {
                //Debug.Log(charObj.GetComponent<NpcCharAbstract>().GetNpcName());
                if (charObj.GetComponent<NpcCharAbstract>().GetNpcName().Equals(characterName))
                {
                    //Debug.Log("found npc and starting dialogue with " + characterName);
                    foundComponent = charObj.GetComponent<StandaloneDialogueComponent>();
                    //charObj.GetComponent<StandaloneDialogueComponent>().StartDialogue(false);
                }

            }

            if (foundComponent != null)
            {

                foundComponent.StartDialogue(false);
            }
            else
            {
                Debug.LogError($"Character name {characterName} not found in NpcCharactersManager characters list");
            }
        }

        //disable all dev npc if we start story mode
        private void DisableTestNPCs(bool isInStoryMode)
        {
            //Debug.Log("npc "+checkpointID);
            if (isInStoryMode)
                foreach (var npc in npcCharactersContainer.transform.GetComponentsInChildren<NpcCharAbstract>())
                {
                    if (npc.GetNpcName().Contains("Test"))
                    {
                        npc.gameObject.SetActive(false);
                    }
                }

        }

        private void CheckAutostartDialogueAndActivate(GameObject npcObject)
        {
            var dialogueComponent = npcObject.GetComponent<StandaloneDialogueComponent>();
            if (dialogueComponent.IsAutoActivationEnabled())
            {
                dialogueComponent.StartDialogue(false);
            }
        }
        
        public List<GameObject> GetNpcCharacters(){
            return _npcCharacters;
        }
    }
}