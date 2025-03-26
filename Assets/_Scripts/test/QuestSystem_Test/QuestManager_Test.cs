using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.test.QuestSystem
{
    public class QuestManager_Test : MonoBehaviour
    {
        [Header("Config")] [SerializeField] private bool loadQuestState = false; //use quest load
        
        //all quests
        private Dictionary<string, Quest_Test> questMap;

        //quest requirements
        private int _currentPlayerLevel;




        //initializes quest "list" with all (pre created) quests from resources folder
        private void Awake()
        {
            questMap = CreateQuestMap();

            /*
            Quest_Test quest = GetQuestById("CollectCoinsQuest_test"); //test - is quest from folder was added
            Debug.Log(quest.info.displayName);
            Debug.Log(quest.info.levelRequirement);
            Debug.Log(quest.stateEnumTest);
            Debug.Log(quest.IsCurrentStepExists());
            */

        }

        private void OnEnable()
        {
            GameEventsManager_Test.Instance.QuestEventsTest.OnStartQuest += StartQuest;
            GameEventsManager_Test.Instance.QuestEventsTest.OnAdvanceQuest += AdvanceQuest;
            GameEventsManager_Test.Instance.QuestEventsTest.OnFinishQuest += FinishQuest;

            GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerLevelChange += PlayerLevelChange; //for test prereq

            GameEventsManager_Test.Instance.QuestEventsTest.OnQuestStepStateChange +=
                QuestStepStateChange; //for saving needs


        }

        private void OnDisable()
        {
            GameEventsManager_Test.Instance.QuestEventsTest.OnStartQuest -= StartQuest;
            GameEventsManager_Test.Instance.QuestEventsTest.OnAdvanceQuest -= AdvanceQuest;
            GameEventsManager_Test.Instance.QuestEventsTest.OnFinishQuest -= FinishQuest;

            GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerLevelChange -= PlayerLevelChange; //for test prereq

            GameEventsManager_Test.Instance.QuestEventsTest.OnQuestStepStateChange -=
                QuestStepStateChange; //for saving needs
        }

        private void Start()
        {
            
            foreach (Quest_Test quest in questMap.Values)
            {
                //initialize any loaded quest steps
                if (quest.StateEnumTest == QuestStateEnum_Test.InProgress)
                {
                    quest.InstantiateCurrentQuestStep(this.transform);
                }
                //broadcast the initial stateEnumTest of all quest on startup
                GameEventsManager_Test.Instance.QuestEventsTest.QuestStateChange(quest);
            }
        }

        private void Update()
        {
            //loop through ALL quests
            foreach (Quest_Test quest in questMap.Values)
            {
                //if we're now meeting the requirements, switch to canStart stateEnumTest
                if (quest.StateEnumTest == QuestStateEnum_Test.RequirementsNotMet && CheckRequirementsMet(quest))
                {
                    ChangeQuestState(quest.info.Id, QuestStateEnum_Test.CanStart);
                }
            }
        }

        private void PlayerLevelChange(int level)
        {
            _currentPlayerLevel = level;
        }

        private bool CheckRequirementsMet(Quest_Test questTest)
        {
            //start true and prove to bne false
            bool meetsRequirements = true;

            //check player level requirements
            if (_currentPlayerLevel < questTest.info.levelRequirement)
            {
                meetsRequirements = false;
            }

            //check questTest prerequisites for completion (if prereq quests have "finished" stateEnumTest, then we can start this questTest)
            foreach (QuestInfoSo_Test prerequisiteQuestInfo in questTest.info.questPrerequisites)
            {
                if (GetQuestById(prerequisiteQuestInfo.Id).StateEnumTest != QuestStateEnum_Test.Finished)
                {
                    meetsRequirements = false;
                    // add this break statement here so that we don't continue on to the next questTest, since we've proven meetsRequirements to be false at this point.
                    break;
                }
            }

            return meetsRequirements;
        }

        private void ChangeQuestState(string id, QuestStateEnum_Test stateEnumTest)
        {
            Quest_Test questTest = GetQuestById(id);
            questTest.StateEnumTest = stateEnumTest;
            GameEventsManager_Test.Instance.QuestEventsTest.QuestStateChange(questTest);
        }

        //instantiating quest step from prefab (into quest manager)
        private void StartQuest(string id)
        {
            Quest_Test questTest = GetQuestById(id);
            questTest.InstantiateCurrentQuestStep(this.transform);
            ChangeQuestState(questTest.info.Id, QuestStateEnum_Test.InProgress);
            Debug.Log("start questTest: " + id);
        }

        private void AdvanceQuest(string id)
        {
            Quest_Test questTest = GetQuestById(id);

            //move on to the next step
            questTest.MoveToNextStep();

            //if there are more steps, instantiate the next one
            if (questTest.IsCurrentStepExists())
            {
                questTest.InstantiateCurrentQuestStep(this.transform);
            }
            else
            {
                ChangeQuestState(questTest.info.Id, QuestStateEnum_Test.CanFinish);
            }


            Debug.Log("advance questTest: " + id);
        }

        //todo add fail quest
        private void FinishQuest(string id)
        {
            Quest_Test questTest = GetQuestById(id);
            ClaimRewards(questTest);
            ChangeQuestState(questTest.info.Id, QuestStateEnum_Test.Finished);

            Debug.Log("finish questTest: " + id);
        }

        //todo to change for universal rewards? (other quests pickup possibilities / open doors etc)
        private void ClaimRewards(Quest_Test questTest)
        {
            GameEventsManager_Test.Instance.GoldEventsTest.GoldGained(questTest.info.goldReward);
            GameEventsManager_Test.Instance.ExpEventsTest.ExperienceGained(questTest.info.expReward);

        }


        private Dictionary<string, Quest_Test> CreateQuestMap()
        {

            //Loads all QuestInfoSo_Test Script objects from asset/resources/quests folder
            QuestInfoSo_Test[] allQuests = Resources.LoadAll<QuestInfoSo_Test>("Quests");

            //Create the quest map current SO.Id + current SO
            Dictionary<string, Quest_Test> idToQuestMap = new Dictionary<string, Quest_Test>();
            foreach (QuestInfoSo_Test questInfo in allQuests)
            {
                if (idToQuestMap.ContainsKey(questInfo.Id))
                {
                    Debug.LogWarning("Duplicate Id found when creating quest map " + questInfo.Id);
                }

                idToQuestMap.Add(questInfo.Id, LoadQuest(questInfo));
            }

            return idToQuestMap;
        }


        private Quest_Test GetQuestById(string id)
        {
            Quest_Test questTest = questMap[id];

            if (questTest == null)
            {
                Debug.LogError("Quest_Test not found in questTest map: " + id);
            }

            return questTest;
        }

        private void QuestStepStateChange(string id, int stepIndex, QuestStepState_Test questStepStateTest)
        {
            Quest_Test questTest = GetQuestById(id);
            questTest.StoreQuestStepState(questStepStateTest, stepIndex);
            ChangeQuestState(id, questTest.StateEnumTest);
        }
        
        private void OnApplicationQuit()
        {
            foreach (Quest_Test quest in questMap.Values)
            {
                /*QuestData_Test questData = quest.GetQuestData();
                Debug.Log(quest.info.Id);
                Debug.Log("stateEnumTest = " + questData.stateEnumTest);
                Debug.Log("index = " + questData.questStepIndex);
        
                foreach (QuestStepState_Test stepState in questData.questStepStates)
                {
                    Debug.Log("step stateEnumTest = " + stepState.stateEnumTest);
                }*/
                
                SaveQuest(quest);
            }
        }
        
        private void SaveQuest(Quest_Test questTest)
        {
            try 
            {
                QuestData_Test questDataTest = questTest.GetQuestData();
                // serialize using JsonUtility
                string serializedData = JsonUtility.ToJson(questDataTest);

                PlayerPrefs.SetString(questTest.info.Id, serializedData); //todo use an actual Save & Load system and write to a file, the cloud, etc..
                
                //Debug.Log(serializedData);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to save questTest with id " + questTest.info.Id + ": " + e);
            }
        }
        
        private Quest_Test LoadQuest(QuestInfoSo_Test questInfo)
        {
            Quest_Test questTest = null;
            try 
            {
                // load questTest from saved data
                if (PlayerPrefs.HasKey(questInfo.Id)&& loadQuestState)
                {
                    string serializedData = PlayerPrefs.GetString(questInfo.Id);
                    QuestData_Test questDataTest = JsonUtility.FromJson<QuestData_Test>(serializedData);
                    questTest = new Quest_Test(questInfo, questDataTest.stateEnumTest, questDataTest.questStepIndex, questDataTest.questStepStates);
                }
                // otherwise, initialize a new questTest
                else 
                {
                    questTest = new Quest_Test(questInfo);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load questTest with id " + questTest.info.Id + ": " + e);
            }
            return questTest;
        }

    }

}