using System;
using System.Collections.Generic;
using _Scripts.Dialog_Ink;
using _Scripts.NarrativeAndCutscenes;
using _Scripts.NarrativeAndCutscenes.Checkpoints;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    /// <summary>
    /// This is narrative controller class. He follows the plot line and controls checkpoints.
    /// Contains and "abstract character" - CutsceneEntity, which responsible for cutscene dialogues (like other NPCs)
    /// Contains cutscene sprites for cutscenes and control methods. //todo to separate? cutscene manager ? (this manager only for progression. Rename game utils to cutscene manager with functions.
    /// Need to show sprites in "Novel view cause of camera settings"
    /// </summary>
    public class StoryProgressionManager : MonoBehaviour
    {
        //add "cutscene methods" for now -> they will temporarily manage cutscene events sequence (show image/amination/run dialogu/start quest/spawn or teleport character).
        //add bool checkpoints? use index with list or [] after checkpoint class implementation (class will be like methods and will be added to list. Class will have states like "not started/started/completed" etc (add events?)
        //to add "cutscene abstract character with dialogs (stories?) like introDialogue1 etc"

        
        public static StoryProgressionManager Instance;
        
        private Dictionary<int, Checkpoint> checkpoints = new Dictionary<int, Checkpoint>();
        private int _currentCheckpointID = 0;

        private GameUtils _gameUtils;
        
        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField] private StandaloneDialogueComponent cutsceneDialogueEntity;
        
        [SerializeField] private GameObject cutsceneEntity;//abstract "npc" with cutscene view dialogues

        [Header("Cutscenes")]
        [SerializeField] private List<TimelineAsset> cutscenes;
        private Dictionary<string, TimelineAsset> _cutsceneDictionary = new Dictionary<string,TimelineAsset>();
        
        
        [Header("Cutscene1Images")]
        [SerializeField]private List<Sprite> cutscene1Sprites = new List<Sprite>();
        private Dictionary<Sprite,bool> _cutscene1SpritesDictionary = new Dictionary<Sprite, bool>();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _gameUtils = new GameUtils();
                InitializeCutsceneSprites();
                InitializeCutsceneDictionary();
                InitializeCheckpoints();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }
        
        
        private void InitializeCutsceneSprites()
        {
            foreach (var sprite in cutscene1Sprites)
            {
                _cutscene1SpritesDictionary[sprite] = false;
            }
        }
        
        private void InitializeCutsceneDictionary()
        {
            foreach (var cutscene in cutscenes)
            {
                _cutsceneDictionary[cutscene.name] = cutscene;
                //Debug.Log(cutscene.name);
            }
        }

        private void OnEnable()
        {
            EventManager.Instance.CutsceneEvents.OnResumeCutscene += ResumeTimeline;
            EventManager.Instance.CutsceneEvents.OnLaunchCutscene += LaunchCutscene;
            
        }
        private void OnDisable()
        {
            EventManager.Instance.CutsceneEvents.OnResumeCutscene -= ResumeTimeline;
            EventManager.Instance.CutsceneEvents.OnLaunchCutscene -= LaunchCutscene;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("1");
                cutsceneEntity.GetComponent<StandaloneDialogueComponent>().StartDialogue(true); 
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("2");
                NextCutsceneImage();
            } 
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("3");
                HideCutsceneImage();
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ShowCutsceneUI();
                Debug.Log("5");
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                HideCutsceneUI();
                Debug.Log("6");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                _gameUtils.TeleportPlayerToNovelPlace("CatherineHouse", "FFCorridor");
                Debug.Log("7");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                StartDialogueWithMother();
                Debug.Log("0");
            }
            /*if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                LaunchIntroCutscene();
                Debug.Log("k0");
            }*/
            
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                EventManager.Instance.LocationsAndPlacesEvents.SetAllPlacesLockState("CatherineHouse", true);
                Debug.Log("lock all places");
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                EventManager.Instance.LocationsAndPlacesEvents.SetAllPlacesLockState("CatherineHouse", false);
                Debug.Log("unlock all places");
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                EventManager.Instance.LocationsAndPlacesEvents.SetPlacesLockState("CatherineHouse", false,
                    CatherineHouseNovelViewPlaces.CatherineRoom.ToString(),
                    CatherineHouseNovelViewPlaces.CatherineDressingRoom.ToString(),
                    CatherineHouseNovelViewPlaces.TFCorridor.ToString(),
                    CatherineHouseNovelViewPlaces.SFCorridor.ToString(),
                    CatherineHouseNovelViewPlaces.FFCorridor.ToString(),
                    CatherineHouseNovelViewPlaces.Kitchen.ToString()
                    );
                Debug.Log("unlock catherine way to kitchen");
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                EventManager.Instance.LocationsAndPlacesEvents.SetAllLocationsLockState(true);
                Debug.Log("lock all locations");
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                EventManager.Instance.LocationsAndPlacesEvents.SetAllLocationsLockState(false);
                Debug.Log("unlock all locations");
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                EventManager.Instance.LocationsAndPlacesEvents.SetLocationLockState("CatherineHouse", true);
                Debug.Log("lock catherine house");
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                EventManager.Instance.DialogueEvents.SetDialogueAutoActivation("Mother", true);
                Debug.Log("auto activate mother dialogue");
            }
        }
        

        public void InitializeCheckpoints() {
            checkpoints.Add(1,new Checkpoint1());
        }

        public void ActivateCheckpoint(int id) {
            if (checkpoints.TryGetValue(id, out var checkpoint)) {
                checkpoint.Activate();
            }

            _currentCheckpointID = id;
            EventManager.Instance.GameEvents.CheckpointActivated(_currentCheckpointID);
        }

        //todo make cutscene as class ?
        public void LaunchCutscene(string cutsceneName)
        {
            EventManager.Instance.CutsceneEvents.CutsceneStarted();
            playableDirector.playableAsset = _cutsceneDictionary[cutsceneName];
            playableDirector.Play();
        }
        


        //------------------game utils methods (to add in cutscene class)----------
        //Cutscene UI
        public void ShowCutsceneUI()
        {
            EventManager.Instance.CutsceneEvents.ShowCutsceneUI();
        }

        public void HideCutsceneUI()
        {
            EventManager.Instance.CutsceneEvents.HideCutsceneBackground();
        }
        
        //set current cutscene sprites (dictionary) method (dict<dict,bool> ?) like in other places
        //add sprites in dictionary
        //display next not displayed sprite (in cutscene ui)
        public void NextCutsceneImage()
        {
            foreach (var pair in _cutscene1SpritesDictionary)
            {
                if (!pair.Value)
                {
                    EventManager.Instance.CutsceneEvents.SetCutsceneImage(pair.Key);
                    EventManager.Instance.CutsceneEvents.ShowCutsceneImage();
                    _cutscene1SpritesDictionary[pair.Key] = true;
                    break; 
                }
            }
        }

        public void HideCutsceneImage()
        {
            EventManager.Instance.CutsceneEvents.HideCutsceneImage();
        }


        //Timeline
        public void PauseTimeline()
        {
            playableDirector.Pause();
        }
        
        public void ResumeTimeline()
        {
            playableDirector.Resume();
        }
        
        public void StopCutscene()
        {
            playableDirector.Stop();
        }



        
        //Dialogues
        
        public void StartNextCutsceneDialogue()
        {
            cutsceneDialogueEntity.StartDialogue(true);
        }
        public void StartDialogueWithMother()
        {
            EventManager.Instance.DialogueEvents.StartDialogueWithNpc("Mother");
        }

        
        //teleports 
        public void TeleportPlayerToKatherineRoom()
        {
            _gameUtils.TeleportPlayerToNovelPlace("CatherineHouse","CatherineRoom");
        }
        
        //utils
        public void DisableHotkeys()
        {
            EventManager.Instance.InputEvents.HotkeysAreActive(false);
        }
        
        public void EnableHotkeys()
        {
            EventManager.Instance.InputEvents.HotkeysAreActive(true);
        }

        public void InvokeStartCutsceneEvent()
        {
            EventManager.Instance.CutsceneEvents.CutsceneStarted();
        }
        public void InvokeFinishCutsceneEvent()
        {
            EventManager.Instance.CutsceneEvents.CutsceneFinished();
        }
    }
}