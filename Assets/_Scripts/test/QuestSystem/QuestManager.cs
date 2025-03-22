using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.test.QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        [Header("Config")] [SerializeField] private bool loadQuestState = false; //use quest load
        
        //all quests
        private Dictionary<string, Quest> questMap;

        //quest requirements
        private int _currentPlayerLevel;




        //initializes quest "list" with all (pre created) quests from resources folder
        private void Awake()
        {
            questMap = CreateQuestMap();

            /*
            Quest quest = GetQuestById("CollectCoinsQuest_test"); //test - is quest from folder was added
            Debug.Log(quest.info.displayName);
            Debug.Log(quest.info.levelRequirement);
            Debug.Log(quest.state);
            Debug.Log(quest.IsCurrentStepExists());
            */

        }

        private void OnEnable()
        {
            GameEventsManager_Test.Instance.QuestEvents.OnStartQuest += StartQuest;
            GameEventsManager_Test.Instance.QuestEvents.OnAdvanceQuest += AdvanceQuest;
            GameEventsManager_Test.Instance.QuestEvents.OnFinishQuest += FinishQuest;

            GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerLevelChange += PlayerLevelChange; //for test prereq

            GameEventsManager_Test.Instance.QuestEvents.OnQuestStepStateChange +=
                QuestStepStateChange; //for saving needs


        }

        private void OnDisable()
        {
            GameEventsManager_Test.Instance.QuestEvents.OnStartQuest -= StartQuest;
            GameEventsManager_Test.Instance.QuestEvents.OnAdvanceQuest -= AdvanceQuest;
            GameEventsManager_Test.Instance.QuestEvents.OnFinishQuest -= FinishQuest;

            GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerLevelChange -= PlayerLevelChange; //for test prereq

            GameEventsManager_Test.Instance.QuestEvents.OnQuestStepStateChange -=
                QuestStepStateChange; //for saving needs
        }

        private void Start()
        {
            
            foreach (Quest quest in questMap.Values)
            {
                //initialize any loaded quest steps
                if (quest.State == QuestState.InProgress)
                {
                    quest.InstantiateCurrentQuestStep(this.transform);
                }
                //broadcast the initial state of all quest on startup
                GameEventsManager_Test.Instance.QuestEvents.QuestStateChange(quest);
            }
        }

        private void Update()
        {
            //loop through ALL quests
            foreach (Quest quest in questMap.Values)
            {
                //if we're noew meeting the requirements, switch to canStart state
                if (quest.State == QuestState.RequirementsNotMet && CheckRequirementsMet(quest))
                {
                    ChangeQuestState(quest.info.Id, QuestState.CanStart);
                }
            }
        }

        private void PlayerLevelChange(int level)
        {
            _currentPlayerLevel = level;
        }

        private bool CheckRequirementsMet(Quest quest)
        {
            //start true and prove to bne false
            bool meetsRequirements = true;

            //check player level requirements
            if (_currentPlayerLevel < quest.info.levelRequirement)
            {
                meetsRequirements = false;
            }

            //check quest prerequisites for completion (if prereq quests have "finished" state, then we can start this quest)
            foreach (QuestInfoSo prerequisiteQuestInfo in quest.info.questPrerequisites)
            {
                if (GetQuestById(prerequisiteQuestInfo.Id).State != QuestState.Finished)
                {
                    meetsRequirements = false;
                    // add this break statement here so that we don't continue on to the next quest, since we've proven meetsRequirements to be false at this point.
                    break;
                }
            }

            return meetsRequirements;
        }

        private void ChangeQuestState(string id, QuestState state)
        {
            Quest quest = GetQuestById(id);
            quest.State = state;
            GameEventsManager_Test.Instance.QuestEvents.QuestStateChange(quest);
        }

        //instantiating quest step from prefab (into quest manager)
        private void StartQuest(string id)
        {
            Quest quest = GetQuestById(id);
            quest.InstantiateCurrentQuestStep(this.transform);
            ChangeQuestState(quest.info.Id, QuestState.InProgress);
            Debug.Log("start quest: " + id);
        }

        private void AdvanceQuest(string id)
        {
            Quest quest = GetQuestById(id);

            //move on to the next step
            quest.MoveToNextStep();

            //if there are more steps, instantiate the next one
            if (quest.IsCurrentStepExists())
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            else
            {
                ChangeQuestState(quest.info.Id, QuestState.CanFinish);
            }


            Debug.Log("advance quest: " + id);
        }

        private void FinishQuest(string id)
        {
            Quest quest = GetQuestById(id);
            ClaimRewards(quest);
            ChangeQuestState(quest.info.Id, QuestState.Finished);

            Debug.Log("finish quest: " + id);
        }

        //todo to change for universal rewards? (other quests pickup possibilities / open doors etc)
        private void ClaimRewards(Quest quest)
        {
            GameEventsManager_Test.Instance.GoldEventsTest.GoldGained(quest.info.goldReward);
            GameEventsManager_Test.Instance.ExpEventsTest.ExperienceGained(quest.info.expReward);

        }


        private Dictionary<string, Quest> CreateQuestMap()
        {

            //Loads all QuestInfoSo Script objects from asset/resources/quests folder
            QuestInfoSo[] allQuests = Resources.LoadAll<QuestInfoSo>("Quests");

            //Create the quest map current SO.Id + current SO
            Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
            foreach (QuestInfoSo questInfo in allQuests)
            {
                if (idToQuestMap.ContainsKey(questInfo.Id))
                {
                    Debug.LogWarning("Duplicate Id found when creating quest map " + questInfo.Id);
                }

                idToQuestMap.Add(questInfo.Id, LoadQuest(questInfo));
            }

            return idToQuestMap;
        }


        private Quest GetQuestById(string id)
        {
            Quest quest = questMap[id];

            if (quest == null)
            {
                Debug.LogError("Quest not found in quest map: " + id);
            }

            return quest;
        }

        private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
        {
            Quest quest = GetQuestById(id);
            quest.StoreQuestStepState(questStepState, stepIndex);
            ChangeQuestState(id, quest.State);
        }
        
        private void OnApplicationQuit()
        {
            foreach (Quest quest in questMap.Values)
            {
                /*QuestData questData = quest.GetQuestData();
                Debug.Log(quest.info.Id);
                Debug.Log("state = " + questData.state);
                Debug.Log("index = " + questData.questStepIndex);
        
                foreach (QuestStepState stepState in questData.questStepStates)
                {
                    Debug.Log("step state = " + stepState.state);
                }*/
                
                SaveQuest(quest);
            }
        }
        
        private void SaveQuest(Quest quest)
        {
            try 
            {
                QuestData questData = quest.GetQuestData();
                // serialize using JsonUtility
                string serializedData = JsonUtility.ToJson(questData);

                PlayerPrefs.SetString(quest.info.Id, serializedData); //todo use an actual Save & Load system and write to a file, the cloud, etc..
                
                //Debug.Log(serializedData);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to save quest with id " + quest.info.Id + ": " + e);
            }
        }
        
        private Quest LoadQuest(QuestInfoSo questInfo)
        {
            Quest quest = null;
            try 
            {
                // load quest from saved data
                if (PlayerPrefs.HasKey(questInfo.Id)&& loadQuestState)
                {
                    string serializedData = PlayerPrefs.GetString(questInfo.Id);
                    QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                    quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
                }
                // otherwise, initialize a new quest
                else 
                {
                    quest = new Quest(questInfo);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load quest with id " + quest.info.Id + ": " + e);
            }
            return quest;
        }

    }

}