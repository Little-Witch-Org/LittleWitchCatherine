using System;
using System.Collections.Generic;
using _Scripts.Dialog_Ink;
using _Scripts.Enums;
using _Scripts.Enums.Places;
using _Scripts.Managers;
using _Scripts.NarrativeAndCutscenes.Cutscenes;
using _Scripts.NarrativeAndCutscenes.Interfaces;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace _Scripts.NarrativeAndCutscenes
{/// <summary>
 /// Manages cutscenes
 /// Class has methods to control and manipulate an objects, game state, characters etc. (almost for cutscenes and narrative events)
 /// Contains and "abstract character" - CutsceneEntity, which responsible for cutscene dialogues (like other NPCs)
 /// Contains cutscene sprites for cutscenes and control methods.
 /// Need to show sprites in "Novel view cause of camera settings"
 /// </summary>
 /// 
    public class CutsceneController :MonoBehaviour
    {
        public static CutsceneController Instance;
        
        [SerializeField] private GameObject cutsceneEntity;//abstract "npc" with cutscene view dialogues
        [SerializeField] private StandaloneDialogueComponent cutsceneDialogueEntity;

        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField] private GameObject cutsceneContainer;
        
        
        [Header("Cutscenes")]
        [SerializeField] private List<GameObject> cutscenesPrefabs;
        
        [Header("Variables")]
        [SerializeField] private AbstractCutscene currentCutsceneScript;
        [SerializeField] private GameObject currentCutsceneGameObject;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        
        
        private void OnEnable()
        {
            EventManager.Instance.CutsceneEvents.OnResumeCutscene += ResumeTimeline;
            EventManager.Instance.CutsceneEvents.OnLaunchCutscene += LaunchCutscene;
            EventManager.Instance.CutsceneEvents.OnCutsceneFinished += FinishCutscene;
            
        }
        

        private void OnDisable()
        {
            EventManager.Instance.CutsceneEvents.OnResumeCutscene -= ResumeTimeline;
            EventManager.Instance.CutsceneEvents.OnLaunchCutscene -= LaunchCutscene;
            EventManager.Instance.CutsceneEvents.OnCutsceneFinished -= FinishCutscene;
        }

        
        private void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("1");
                LaunchCutscene("TableTimeSkipCutscene10");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("2");
                EventManager.Instance.LocationsAndPlacesEvents.UpdatePlaceStateSprite("CatherineRoom");
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
                TeleportPlayerToNovelPlace("CatherineHouse", "FFCorridor");
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
            }#1#
            
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
                    CatherineHouseNovelViewPlacesEnum.CatherineRoom.ToString(),
                    CatherineHouseNovelViewPlacesEnum.CatherineDressingRoom.ToString(),
                    CatherineHouseNovelViewPlacesEnum.TFCorridor.ToString(),
                    CatherineHouseNovelViewPlacesEnum.SFCorridor.ToString(),
                    CatherineHouseNovelViewPlacesEnum.FFCorridor.ToString(),
                    CatherineHouseNovelViewPlacesEnum.Kitchen.ToString()
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
            }*/
        }
        
        
        private void LaunchCutscene(string cutsceneName)
        {
            var cutsceneGo = cutscenesPrefabs.Find(x => x.name == cutsceneName);
            if (cutsceneGo == null)
            {
                Debug.LogError($"Cutscene {cutsceneName} not found");
                return;
            }
            currentCutsceneGameObject = Instantiate(cutsceneGo, cutsceneContainer.transform, false);
            
            currentCutsceneScript = currentCutsceneGameObject.GetComponent<AbstractCutscene>();
            
            currentCutsceneScript.LaunchCutscene(playableDirector);
            
        }
        
        private void FinishCutscene()
        {
            Destroy(currentCutsceneGameObject);
            currentCutsceneScript = null;
            playableDirector.playableAsset = null;
        }
        

        
        //Cutscene UI
        public void ShowCutsceneUI()
        {
            //Debug.Log("ShowCutsceneUI");
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
            currentCutsceneScript.DisplayNextImage();
        }
        
        public void NextCutsceneImageWithFade() 
        {
            currentCutsceneScript.DisplayNextImageWithFade();
        }

        public void ShowCutsceneImage()
        {
            EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
        }
        public void HideCutsceneImage()
        {
            EventManager.Instance.CutsceneEvents.HideCutsceneImage(false,0);
        }
        
        public void ShowCutsceneImageWithFade()
        {
            EventManager.Instance.CutsceneEvents.ShowCutsceneImage(true,0.5f);
        }
        public void HideCutsceneImageWithFade()
        {
            EventManager.Instance.CutsceneEvents.HideCutsceneImage(true,0.5f);
        }
        
        public void ShowCutsceneBackingPanel()
        {
            EventManager.Instance.CutsceneEvents.ShowBackingPanel(false,0);
        }
        
        public void HideCutsceneBackingPanel()
        {
            EventManager.Instance.CutsceneEvents.HideBackingPanel(false,0);
        }
        
        public void ShowCutsceneBackingPanelWithFade()
        {
            EventManager.Instance.CutsceneEvents.ShowBackingPanel(true,0.5f);
        }
        
        public void HideCutsceneBackingPanelWithFade()
        {
            EventManager.Instance.CutsceneEvents.HideBackingPanel(true,0.5f);
        }
        
        public void SetCurrentPlaceSpriteToCutsceneImage()
        {
            var placeSprite = EventManager.Instance.LocationsAndPlacesEvents.GetCurrentPlaceSprite();
            EventManager.Instance.CutsceneEvents.SetCutsceneImage(placeSprite);
            EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
        }
        
        public void SetCurrentPlaceSpriteToCutsceneImageWithFade() 
        {
            var placeSprite = EventManager.Instance.LocationsAndPlacesEvents.GetCurrentPlaceSprite();
            currentCutsceneScript.DisplayCustomImageWithFade(placeSprite);
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
        
        //starts dialogue with cutscene entity
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
            TeleportPlayerToNovelPlace("CatherineHouse","CatherineRoom");
        }
        
        public void TeleportPlayerToNovelPlace(string location, string place)
        {
            //PlayerCharacterManager.Instance.SetPreviousSpawnPositionPoint(pointNamesName); //
            EventManager.Instance.TransitionEvents.PlaceTransitionTrigger(location, place);
            EventManager.Instance.TransitionEvents.ChangeScene(SceneNamesEnum.NovelView);
        }
        
        //utils
        public void DisableHotkeys()
        {
            EventManager.Instance.InputEvents.SetHotkeysActive(false);
        }
        
        public void EnableHotkeys()
        {
            EventManager.Instance.InputEvents.SetHotkeysActive(true);
        }
        
        public void DisableInput()
        {
            EventManager.Instance.InputEvents.SetInputActive(false);
        }
        
        public void EnableInput()
        {
            EventManager.Instance.InputEvents.SetInputActive(true);
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