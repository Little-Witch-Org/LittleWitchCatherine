using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using _Scripts.Service.Log;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.QuestSystem
{
    /// <summary>
    ///Spawner OnStepCreated gets triggers for current step and instantiates disabled objects (quest step triggers)
    ///In Update() spawner checks triggers prerequisites and compares it with current game situation (location/place/time..)
    ///Enables trigger object when it needs to
    ///Quest step trigger disable self after trigger activates
    ///When step is over (deleted), spawner deletes parent object which stores trigger objects (with triggers). Also delete links on deleted triggers. 
    /// //todo need to handle pre-created trigger (like open box in scene). Need to add trigger that will be connect to this object and react on activation.
    /// </summary>
    public class QuestTriggerSpawner : MonoBehaviour
    {

        //[SerializeField] private string questName;
        //[SerializeField] private Transform questManager;

        private List<GameObject> _triggersOnScene = new List<GameObject>();
        private List<GameObject> _triggersToRemove = new List<GameObject>(); //create trigger links that will be removed from list after we delete it from scene
        private List<GameObject> _triggersToSpawn = new List<GameObject>(); //copy triggers list (secure dynamic nature of trigger list)

        private void OnEnable()
        {
            EventManager.Instance.QuestEvents.OnQuestStepCreated += HandleStepCreated;
            EventManager.Instance.QuestEvents.OnQuestStepDeleted += HandleStepDeleted;

        }

        private void OnDestroy()
        {
            EventManager.Instance.QuestEvents.OnQuestStepCreated -= HandleStepCreated;
            EventManager.Instance.QuestEvents.OnQuestStepDeleted -= HandleStepDeleted;

        }

        private void Update() //TODO use events instead of it ( like in npc manager) ?
        {
            if (_triggersOnScene.Count > 0)
            {
                CheckSpawnConditions();
            }
        }

        private void HandleStepCreated(GameObject questStep) //todo handle this
        {
            // get quest folder name (quest step without StepX)
            string questFolder = questStep.name.Split(new[] { "Step" }, StringSplitOptions.None)[0];
            string path = $"Quests/DevQuests/{questFolder}/Triggers";
            
            GameObject[]
                allTriggers = UnityEngine.Resources.LoadAll<GameObject>(path); //temp list of possible trigger prefabs
            
            string path1 = $"Quests/StoryQuests/{questFolder}/Triggers"; 
            //Debug.Log(path1);
            
            GameObject[]
                allTriggers1 = UnityEngine.Resources.LoadAll<GameObject>(path1); //temp list of possible trigger prefabs


            allTriggers = allTriggers.Concat(allTriggers1).ToArray();
            
            if (allTriggers.Length == 0)
            {
                QuestDebug.Instance.Log($"No triggers found for {questStep} at path: {path}");
                return;
            }



            // create container obj with triggers
            string containerName = questStep.name + "_Triggers";
            Transform container = transform.Find(containerName); //do we need this ?

            if (container == null)
            {
                GameObject containerObj = new GameObject(containerName);
                container = containerObj.transform;
                container.SetParent(transform);
                container.localPosition = Vector3.zero;
            }

            string cleanStepName = questStep.name.Replace("(Clone)", "");
            
            // Filter and instantiate only matching triggers
            foreach (GameObject triggerPrefab in allTriggers)
            {
                //Check if trigger name starts with questStep name //handle Step10+ ?
                if (triggerPrefab.name.StartsWith(cleanStepName))
                {
                    //instant quest trigger prefab
                    GameObject triggerInstance = Instantiate(triggerPrefab, container);
                    triggerInstance.SetActive(false);
                    _triggersOnScene.Add(triggerInstance);
                }

            }
            
            QuestDebug.Instance.Log("Triggers in the 'OnScene' list (step created method) = "+ _triggersOnScene.Count);
        }

        private void HandleStepDeleted(GameObject questStep)
        {
            
            //check that our triggers links list contains finished step and delete dependent triggers and folder. 
            foreach (var trigger in _triggersOnScene)
            {
                QuestStepTrigger
                    triggerScript = trigger.GetComponent<QuestStepTrigger>(); //step must contain QuestStepTrigger.cs

                if (_triggersOnScene != null)
                {
                    if (questStep.name.StartsWith(triggerScript.questStep.name))
                    {
                        Destroy(trigger.gameObject);
                        _triggersToRemove.Add(trigger);
                    }
                }
            }

            //delete links from list after we delete objects
            _triggersOnScene.RemoveAll(trigger => _triggersToRemove.Contains(trigger));

            QuestDebug.Instance.Log("Triggers in the 'OnScene' list (step delete method) = "+ _triggersOnScene.Count);

            //delete trigger container object
            string containerName = questStep.name + "_Triggers";
            var container = transform.Cast<Transform>()
                .FirstOrDefault(child => child.name == containerName);

            if (container != null)
            {
                Destroy(container.gameObject);
            }
            _triggersToRemove.Clear();
        }

        private void CheckSpawnConditions()
        {
            
            _triggersToSpawn.AddRange(_triggersOnScene);

            //check all links
            foreach (var triggerObject in _triggersToSpawn)
            {
                var triggerEnableLocation = triggerObject.GetComponent<QuestStepTrigger>().locationName;
                var triggerEnablePlace = triggerObject.GetComponent<QuestStepTrigger>().placeName;

                var currentCharLocation = PlayerCharacterManager.Instance.GetCurrentLocation();
                var currentCharPlace = PlayerCharacterManager.Instance.GetCurrentNovelPlace();

                var isTriggered = triggerObject.GetComponent<QuestStepTrigger>().isTriggered;
                
                
                /*
                Debug.Log(currentCharLocation);
                Debug.Log(currentCharPlace);
                Debug.Log(triggerEnableLocation);
                Debug.Log(triggerEnablePlace);
                Debug.Log(isTriggered);
                */
                
                
                //enable if conditions match and not triggered
                if (triggerEnableLocation.Equals(currentCharLocation) && triggerEnablePlace.Equals(currentCharPlace)&& !isTriggered)
                {
                    EnableTrigger(triggerObject);
                }
            }

            _triggersToSpawn.Clear();
        }

        private void EnableTrigger(GameObject questStepTrigger)
        {
            questStepTrigger.SetActive(true);
        }
    }

}
