using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.QuestSystem;
using _Scripts.QuestSystem.UI;
using _Scripts.Service.Log;
using UnityEngine;

namespace _Scripts.Managers
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance;
        [Header("Config")]
        [SerializeField] private bool loadQuestState = false; //use quest load
        
        //all quests
        private Dictionary<string, Quest> _questMap;

        //quest requirements
        private int _currentPlayerLevel;
        
        private void Awake()
        {
            if (Instance == null)
            {
                //initializes quest "list" with all (pre created) quests from resources folder
                _questMap = CreateQuestMap();
                
                Instance = this;
                DontDestroyOnLoad(gameObject);
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

            EventManager.Instance.QuestEvents.OnQuestStepDataChange +=
                QuestStepDataChange; //change values for current quest step

            EventManager.Instance.QuestEvents.OnQuestByQuestInfoSoRequest += GetQuestFromMap;
            EventManager.Instance.QuestEvents.OnQuestByQuestIdRequest += GetQuestById;
            
            EventManager.Instance.QuestEvents.OnQuestAvailabilityChange += ChangeQuestAvailability;
            EventManager.Instance.QuestEvents.OnQuestVisibilityChange += ChangeQuestVisibility;
            
            EventManager.Instance.QuestEvents.OnFinishCurrentQuestStep += FinishCurrentQuestStep;

        }

        private void OnDisable()
        {
            EventManager.Instance.QuestEvents.OnStartQuest -= StartQuest;
            EventManager.Instance.QuestEvents.OnAdvanceQuest -= AdvanceQuest;
            EventManager.Instance.QuestEvents.OnFinishQuest -= FinishQuest;

            //GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerLevelChange -= PlayerLevelChange; //for test prereq

            EventManager.Instance.QuestEvents.OnQuestStepDataChange -=
                QuestStepDataChange; //for saving needs
            
            EventManager.Instance.QuestEvents.OnQuestByQuestInfoSoRequest -= GetQuestFromMap;
            EventManager.Instance.QuestEvents.OnQuestByQuestIdRequest -= GetQuestById;
            EventManager.Instance.QuestEvents.OnQuestAvailabilityChange -= ChangeQuestAvailability;
            EventManager.Instance.QuestEvents.OnQuestVisibilityChange -= ChangeQuestVisibility;
            
            EventManager.Instance.QuestEvents.OnFinishCurrentQuestStep -= FinishCurrentQuestStep;
        }

        private void Start()
        {
            
            
            foreach (Quest quest in _questMap.Values)
            {
                //initialize any loaded quest steps
                if (quest.StateEnum == QuestStateEnum.InProgress)
                {
                    quest.InstantiateCurrentQuestStep(this.transform);
                }
                //broadcast the initial stateEnum of all quest on startup
                EventManager.Instance.QuestEvents.QuestStateChanged(quest);
            }
        }

        //quest state used almost to update info in UI
        private void Update() //todo refactor this to event based ?
        {
            //loop through ALL quests
            foreach (Quest quest in _questMap.Values)
            {
                //if quest has status "not met" and we're now meeting the requirements, switch to canStart stateEnum
                if (quest.StateEnum == QuestStateEnum.RequirementsNotMet && CheckRequirementsMet(quest))
                {
                    ChangeQuestState(quest.InfoSo.Id, QuestStateEnum.CanStart);
                }
                
                //change CanStart to RNM (if we disable quest)
                if (quest.StateEnum == QuestStateEnum.CanStart && !CheckRequirementsMet(quest))
                {
                    ChangeQuestState(quest.InfoSo.Id, QuestStateEnum.RequirementsNotMet);
                }
            }
        }
        
        //manual method for change availability of quest
        private void ChangeQuestAvailability(string questId,bool isAvailable)
        {
            GetQuestById(questId).IsQuestAvailable = isAvailable;
        }
        
        //manual method for change visibility of Quest
        private void ChangeQuestVisibility(string quesId,bool isVisible)
        {
            var quest = GetQuestById(quesId);
            
            quest.IsQuestVisible = isVisible;

            EventManager.Instance.QuestEvents.UpdateQuestVisibilityInUI();
        }
        
        //get Quest from map by questInfoSo
        private Quest GetQuestFromMap(QuestInfoSo questSoParam)
        {
            //Debug.Log("provided parameter questSO: " + questSoParam);
            foreach (Quest quest in _questMap.Values)
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
            EventManager.Instance.QuestEvents.QuestStateChanged(quest);
        }

        //Instantiating quest step from prefab (into quest manager). Any existed quest can be started from event and avoid RNM(+available) state, but check warnings!
        private void StartQuest(string id)
        {
            Quest quest = GetQuestById(id);

            if (!quest.IsQuestVisible)
            {
                QuestDebug.Instance.LogWarning($"Quest {id} is not visible!");
            }
            
            if (quest.StateEnum.Equals(QuestStateEnum.RequirementsNotMet)) //
            {
                QuestDebug.Instance.LogWarning($"Quest {id} requirements not met!");
            }
            
            quest.InstantiateCurrentQuestStep(this.transform);
            ChangeQuestState(quest.InfoSo.Id, QuestStateEnum.InProgress);
            QuestDebug.Instance.Log("Start quest: " + id);
        }

        private void AdvanceQuest(string id)
        {
            Quest quest = GetQuestById(id);

            //move on to the next step index
            quest.IncrementQuestStepIndex();

            //if there are more steps, instantiate the next one
            if (quest.IsCurrentStepExists())
            {
                quest.InstantiateCurrentQuestStep(this.transform); //if invokes autoFail without delay, it may be some issues with AdvanceQuest logs priority
                QuestDebug.Instance.Log($"Quest advanced: {id} with new step {quest.GetCurrentQuestStepGameObject()?.GetComponent<QuestStep>().name}");
            }
            else
            {
                QuestDebug.Instance.Log($"Quest advanced: {id}. No new steps found. Quest can be finished" );
                ChangeQuestState(quest.InfoSo.Id, QuestStateEnum.CanFinish);
            }
        }

        //todo add partial finish option
        //Finishes quest. Quest must be finished manually from any place after all steps have been finished.
        private void FinishQuest(string id)
        {
            Quest quest = GetQuestById(id);
            
            if (!quest.StateEnum.Equals(QuestStateEnum.CanFinish)) //
            {
                QuestDebug.Instance.LogWarning($"Quest {id} is not in CanFinish state!");
            }
            
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
            
            QuestDebug.Instance.Log($"Failed steps count: {failedStepsCount} for quest {id}");

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
            Quest quest = _questMap[id];

            if (quest == null)
            {
                Debug.LogError("Quest not found in quest map: " + id);
            }

            return quest;
        }

        private void QuestStepDataChange(string id, int stepIndex, QuestStepData questStepData)
        {
            Quest quest = GetQuestById(id);
            quest.StoreQuestStepValues(questStepData, stepIndex);
            ChangeQuestState(id, quest.StateEnum);
        }

        private void FinishCurrentQuestStep(string questId, bool isFailed)
        {
            Quest quest = GetQuestById(questId);
            var step = quest.GetCurrentQuestStepGameObject()?.GetComponent<QuestStep>();
            if (step == null)
            {
                QuestDebug.Instance.LogError("Current quest step doesn't exist: " + questId); //handles method invocation if quest already finished.
                return;
            }

            step.FinishQuesStep(isFailed);
        }
        
        private void OnApplicationQuit()
        {
            foreach (Quest quest in _questMap.Values)
            {
                /*QuestData questData = quest.GetQuestData();
                Debug.Log(quest.InfoSo.Id);
                Debug.Log("stateEnum = " + questData.stateEnum);
                Debug.Log("index = " + questData.questStepIndex);
        
                foreach (QuestStepData stepState in questData.questStepValues)
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