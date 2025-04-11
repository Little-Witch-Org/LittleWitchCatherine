using System.Collections.Generic;
using _Scripts.Service.Log;
using UnityEngine;

namespace _Scripts.QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance;
        [Header("Config")]
        [SerializeField] private bool loadQuestState = false; //use quest load
        
        //all quests
        private Dictionary<string, Quest> questMap;

        //quest requirements
        private int _currentPlayerLevel;
        

        //initializes quest "list" with all (pre created) quests from resources folder
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                questMap = CreateQuestMap();
            }
            else
            {
                Destroy(gameObject);
            }

            /*
            Quest quest = GetQuestById("CollectCoinsQuest_test"); //test - is quest from folder was added
            Debug.Log(quest.InfoSo.displayName);
            Debug.Log(quest.InfoSo.levelRequirement);
            Debug.Log(quest.stateEnum);
            Debug.Log(quest.IsCurrentStepExists());
            */

        }
      private void OnEnable()
        {
            EventManager.Instance.QuestEvents.OnStartQuest += StartQuest;
            EventManager.Instance.QuestEvents.OnAdvanceQuest += AdvanceQuest;
            EventManager.Instance.QuestEvents.OnFinishQuest += FinishQuest;

            //GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerLevelChange += PlayerLevelChange; //for test prereq

            EventManager.Instance.QuestEvents.OnQuestStepValuesChange +=
                QuestStepValuesChange; //for saving needs

            EventManager.Instance.QuestEvents.OnRequestQuestByQuestInfoSo += GetQuestFromMap;
            
            EventManager.Instance.QuestEvents.OnQuestAvailabilityChange += ChangeQuestAvailability;

        }

        private void OnDisable()
        {
            EventManager.Instance.QuestEvents.OnStartQuest -= StartQuest;
            EventManager.Instance.QuestEvents.OnAdvanceQuest -= AdvanceQuest;
            EventManager.Instance.QuestEvents.OnFinishQuest -= FinishQuest;

            //GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerLevelChange -= PlayerLevelChange; //for test prereq

            EventManager.Instance.QuestEvents.OnQuestStepValuesChange -=
                QuestStepValuesChange; //for saving needs
            
            EventManager.Instance.QuestEvents.OnRequestQuestByQuestInfoSo -= GetQuestFromMap;
            EventManager.Instance.QuestEvents.OnQuestAvailabilityChange -= ChangeQuestAvailability;
        }

        private void Start()
        {
            
            foreach (Quest quest in questMap.Values)
            {
                //initialize any loaded quest steps
                if (quest.StateEnum == QuestStateEnum.InProgress)
                {
                    quest.InstantiateCurrentQuestStep(this.transform);
                }
                //broadcast the initial stateEnum of all quest on startup
                EventManager.Instance.QuestEvents.QuestStateChange(quest);
            }
        }

        private void Update()
        {
            //loop through ALL quests
            foreach (Quest quest in questMap.Values)
            {
                //if quest has status "not met" and we're now meeting the requirements, switch to canStart stateEnum
                if (quest.StateEnum == QuestStateEnum.RequirementsNotMet && CheckRequirementsMet(quest))
                {
                    ChangeQuestState(quest.InfoSo.Id, QuestStateEnum.CanStart);
                }
            }
        }
        
        //manual method for change availability of questOS
        private void ChangeQuestAvailability(string questSoId,bool isAvailable)
        {
            GetQuestById(questSoId).IsQuestAvailable = isAvailable;
        }
        
        //get Quest from map by questInfoSo
        private Quest GetQuestFromMap(QuestInfoSo questSoParam)
        {
            //Debug.Log("provided parameter questSO: " + questSoParam);
            foreach (Quest quest in questMap.Values)
            {
                //Debug.Log(quest.InfoSo);
                if (quest.InfoSo.Id == questSoParam.Id)
                {
                    //Debug.Log(quest.InfoSo + "is found");
                    return quest;
                }
            }
            Debug.LogError(questSoParam.Id + " is not found");
            return null;
        }

        private bool CheckRequirementsMet(Quest quest)
        {
            //check is questSo is available (on/off)
            bool meetsRequirements = quest.IsQuestAvailable;
            //Debug.Log("In manager check requirements -> quest is available = "+ meetsRequirements);

            //todo handle custom requirements

            //todo quest can be failed. Add new variable - canStartDependingQuest if previous failed?
            //check quest prerequisites for completion (if prereq quests have "finished" stateEnum, then we can start this quest)
            foreach (QuestInfoSo prerequisiteQuestInfo in quest.InfoSo.questPrerequisites)
            {
                if (GetQuestById(prerequisiteQuestInfo.Id).StateEnum != QuestStateEnum.Finished)
                {
                    meetsRequirements = false;
                    // add this break statement here so that we don't continue on to the next quest, since we've proven meetsRequirements to be false at this point.
                    break;
                }
            }

            return meetsRequirements;
        }

        private void ChangeQuestState(string id, QuestStateEnum stateEnum)
        {
            Quest quest = GetQuestById(id);
            quest.StateEnum = stateEnum;
            EventManager.Instance.QuestEvents.QuestStateChange(quest);
        }

        //instantiating quest step from prefab (into quest manager)
        private void StartQuest(string id)
        {
            Quest quest = GetQuestById(id);
            quest.InstantiateCurrentQuestStep(this.transform);
            ChangeQuestState(quest.InfoSo.Id, QuestStateEnum.InProgress);
            QuestDebug.Instance.Log("Start quest: " + id);
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
                ChangeQuestState(quest.InfoSo.Id, QuestStateEnum.CanFinish);
            }


            QuestDebug.Instance.Log("Advance quest: " + id);
        }

        //todo add partial finish option
        private void FinishQuest(string id)
        {
            Quest quest = GetQuestById(id);
            
            //check failed steps
            var questData = quest.GetQuestData();
            int failedStepsCount = 0;
            foreach (var stepValue in questData.questStepValues)
            {
                if (stepValue.isFailed)
                {
                    failedStepsCount++;
                }
            }
            
            Debug.Log("Failed steps count: " + failedStepsCount);

            if (failedStepsCount == 0)
            {
                ChangeQuestState(quest.InfoSo.Id, QuestStateEnum.Finished);
                ClaimRewards(quest);
            }

            if (failedStepsCount > 0)
            {
                ChangeQuestState(quest.InfoSo.Id, QuestStateEnum.Failed);
            }
            

            Debug.Log("finish quest: " + id);
            
        }

        //todo to change for universal rewards? (other quests pickup possibilities / open doors etc)
        private void ClaimRewards(Quest quest)
        {
            //GameEventsManager_Test.Instance.GoldEventsTest.GoldGained(quest.InfoSo.goldReward);
            //GameEventsManager_Test.Instance.ExpEventsTest.ExperienceGained(quest.InfoSo.expReward);
            
            Debug.Log("Claimed reward for quest: " + quest.InfoSo.Id);

        }


        private Dictionary<string, Quest> CreateQuestMap()
        {

            //Loads all QuestInfoSo Script objects from asset/resources/quests folder
            QuestInfoSo[] allQuests = UnityEngine.Resources.LoadAll<QuestInfoSo>("Quests");

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

        private void QuestStepValuesChange(string id, int stepIndex, QuestStepValues questStepValues)
        {
            Quest quest = GetQuestById(id);
            quest.StoreQuestStepInfoValues(questStepValues, stepIndex);
            ChangeQuestState(id, quest.StateEnum);
        }
        
        private void OnApplicationQuit()
        {
            foreach (Quest quest in questMap.Values)
            {
                /*QuestData questData = quest.GetQuestData();
                Debug.Log(quest.InfoSo.Id);
                Debug.Log("stateEnum = " + questData.stateEnum);
                Debug.Log("index = " + questData.questStepIndex);
        
                foreach (QuestStepValues stepState in questData.questStepValues)
                {
                    Debug.Log("step stateEnum = " + stepState.stateEnum);
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

                PlayerPrefs.SetString(quest.InfoSo.Id, serializedData); //todo use an actual Save & Load system and write to a file, the cloud, etc..
                
                //Debug.Log(serializedData);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to save quest with id " + quest.InfoSo.Id + ": " + e);
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
                    quest = new Quest(questInfo, questData.stateEnum, questData.questStepIndex, questData.questStepValues);
                }
                // otherwise, initialize a new quest
                else 
                {
                    quest = new Quest(questInfo);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load quest with id " + quest.InfoSo.Id + ": " + e);
            }
            return quest;
        }

    }

}