using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Characters.NPC;
using _Scripts.Dialog_Ink;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Managers
{
    /// <summary>
    /// Script contains all npc characters (for novel view now).
    /// Enables and disables npc (body) according to conditions (place/time etc) using events
    /// NPC move methods.
    /// </summary>
    public class NpcCharactersManager : MonoBehaviour
    {
        public static NpcCharactersManager Instance;
        
        [SerializeField] private List<GameObject> npcCharacterPrefabs;
        [SerializeField] private GameObject npcCharactersContainer;
        private List<GameObject>  _npcCharacters = new ();

        private Coroutine _autoStartDialogueCoroutine;

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
            
            EventManager.Instance.NpcEvents.OnSetNpcIgnoredStatus +=SetNpcIgnoredPositionStatus;
            EventManager.Instance.NpcEvents.OnMoveNpc +=MoveNpc;

            EventManager.Instance.NpcEvents.OnDeleteNpc += DeleteNpcCharacter;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.TransitionEvents.OnCurrentPlaceOnScreen -= CheckSpawnConditions;
            EventManager.Instance.DialogueEvents.OnStartDialogueWithNpc -= StartDialogueWithCurrentNpc;
            
            EventManager.Instance.GameEvents.OnStoryModActivated -= DisableTestNPCs;
            
            EventManager.Instance.NpcEvents.OnSetNpcIgnoredStatus -=SetNpcIgnoredPositionStatus;
            EventManager.Instance.NpcEvents.OnMoveNpc -=MoveNpc;
            
            EventManager.Instance.NpcEvents.OnDeleteNpc -= DeleteNpcCharacter;

            
            if (_autoStartDialogueCoroutine != null)
            {
                StopCoroutine(_autoStartDialogueCoroutine);
            }
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
            //var currentOnScreenLocation = TransitionManager.Instance.GetCurrentLocation();
            //var currentOnScreenPlace = TransitionManager.Instance.GetCurrentPlace();

            
            //check all Npc's
            foreach (var npcObject in _npcCharacters)
            {
                var npcLocation = npcObject.GetComponent<NpcCharAbstract>().GetCurrentLocationName();
                var npcPlace = npcObject.GetComponent<NpcCharAbstract>().GetCurrentPlaceName();

                //Debug.Log(npcObject.GetComponent<NpcCharAbstract>().GetCurrentLocationName());
                //Debug.Log(npcObject.GetComponent<NpcCharAbstract>().GetCurrentPlaceName());
                
                //enable/disable npc body according to scene state (and if isPositionIgnored = false)
                if (npcLocation.Equals(location) &&
                    npcPlace.Equals(place) && !npcObject.GetComponent<NpcCharAbstract>().GetPositionIgnored())
                {
                    
                    //Debug.LogFormat("Npc character \"{0}\"enabled",npcObject.GetComponent<NpcCharAbstract>().GetNpcName());
                    EnableNpcBody(npcObject);
                    
                }
                else
                {
                    if (npcObject.GetComponent<NpcCharAbstract>().GetNpcBodyIsActive())
                    {
                        //Debug.LogFormat("Npc character \"{0}\"disabled",npcObject.GetComponent<NpcCharAbstract>().GetNpcName());
                        DisableNpcBody(npcObject);
                    }
                }

                //activate dialogue if auto activate option is true and player in current location/place
                if (npcLocation.Equals(location) &&
                    npcPlace.Equals(place))
                {
                    //activate dialogue if auto dialogue activation is enabled
                    _autoStartDialogueCoroutine= StartCoroutine(CheckAutostartDialogueAndActivateDelayed(npcObject));
                }
            }
        }

        private void EnableNpcBody(GameObject npcObject)
        {
            npcObject.GetComponent<NpcCharAbstract>().EnableBody();
        }

        private void DisableNpcBody(GameObject npcObject)
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

        private IEnumerator CheckAutostartDialogueAndActivateDelayed(GameObject npcObject)
        {
            //get place change fade (out) animation duration
            var duration = TransitionManager.Instance.GetPlaceFadeDuration();
            
            var dialogueComponent = npcObject.GetComponent<StandaloneDialogueComponent>();
            if (dialogueComponent.IsAutoActivationEnabled())
            {
                yield return new WaitForSeconds(duration+0.1f);//(+0.1) handles race condition with "enable hotkeys" after transition
                dialogueComponent.StartDialogue(false);
            }
        }
        
        public List<GameObject> GetNpcCharacters(){
            return _npcCharacters;
        }

        //using linq
        private void SetNpcIgnoredPositionStatus(string npcName, bool status)
        {
            GetNpcCharacters()
                .Select(npc => npc.GetComponent<NpcCharAbstract>())
                .FirstOrDefault(npc => npc != null && npc.GetNpcName() == npcName)?
                .SetPositionIgnored(status);
            
            //if invokes in current location
            var currentOnScreenLocation = TransitionManager.Instance.GetCurrentLocation();
            var currentOnScreenPlace = TransitionManager.Instance.GetCurrentPlace();
            CheckSpawnConditions(currentOnScreenLocation, currentOnScreenPlace);
        }

        //using foreach
        /*private void SetNpcIgnoredPositionStatus(string npcName, bool status)
        {
            foreach (var npc in GetNpcCharacters())
            {
                var npcChar = npc.GetComponent<NpcCharAbstract>();
                if (npcChar != null && npcChar.GetNpcName() == npcName)
                {
                    npcChar.SetPositionIgnored(status);
                    return;  // Выходим после нахождения нужного NPC
                }
            }
        }*/

        public void DeleteNpcCharacter(String npcName)
        {
            var npcGo = _npcCharacters.Find(n => n.GetComponent<NpcCharAbstract>().GetNpcName().Equals(npcName));
            if (npcGo != null)
            {
                _npcCharacters.Remove(npcGo);
                Destroy(npcGo);
            }
            else
            {
                Debug.LogError($"NPC character {npcName} not found. Cant delete this character.");
            }
        }

        //sets new place and location for npc and update spawning
        public void MoveNpc(string npcName, string location, string place)
        {
            var npc = GetNpcCharacters()
                .Select(npc => npc.GetComponent<NpcCharAbstract>())
                .FirstOrDefault(npc => npc != null && npc.GetNpcName() == npcName);

            if (npc == null)
            {
                Debug.LogError($"NPC character {npcName} not found. Cant move this character.");
                return;
            }
            
            npc.SetLocationAndPlace(location, place);
            
            //if invokes in current location
            var currentOnScreenLocation = TransitionManager.Instance.GetCurrentLocation();
            var currentOnScreenPlace = TransitionManager.Instance.GetCurrentPlace();
            CheckSpawnConditions(currentOnScreenLocation, currentOnScreenPlace);
        }

    }
}