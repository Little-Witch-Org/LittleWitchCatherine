using System;
using _Scripts.Enums;
using _Scripts.Service.Log;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;


namespace _Scripts.QuestSystem
{
    /// <summary>
    /// Represents quest entity which contain QuestInfoSo and current quest status. Is contained in the dictionary of the quest manager.
    /// Quest step data are separated from steps. Data for current step can be reach by using step index.
    /// </summary>
    public class Quest
    {
        //static InfoSo
        public QuestInfoSo InfoSo;
        
        //initializes from QuestSo and used in runtime(in QuestSo this variable value is serializable and must not be changed)
        public bool IsQuestAvailable;
        
        public bool IsQuestVisible;
    
        //stateEnum InfoSo
        public QuestStateEnum StateEnum;
    
        private int _currentQuestStepIndex;
        
        private GameObject _currentQuestStepGameObject;
    
        private readonly QuestStepData[] _questStepData;

        public Quest(QuestInfoSo questInfoSo)
        {
            this.InfoSo = questInfoSo;
            this.IsQuestAvailable = questInfoSo.isQuestAvailable;
            this.IsQuestVisible = questInfoSo.isQuestVisible;
            this.StateEnum = QuestStateEnum.RequirementsNotMet;
            this._currentQuestStepIndex = 0;
            this._questStepData = new QuestStepData[InfoSo.questStepPrefabs.Length];

            for (int i = 0; i < _questStepData.Length; i++)//initialising states for every quest steps
            {
                _questStepData[i] = new QuestStepData();
            }
        }
    
        //for load needs
        public Quest(QuestInfoSo questInfoSo, QuestStateEnum questStateEnum, int currentQuestStepIndex, QuestStepData[] questStepData)
        {
            this.InfoSo = questInfoSo;
            this.StateEnum = questStateEnum;
            this._currentQuestStepIndex = currentQuestStepIndex;
            this._questStepData = questStepData;

            // if the quest step states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.
            if (this._questStepData.Length != this.InfoSo.questStepPrefabs.Length)
            {
                Debug.LogWarning("Quest Step Prefabs and Quest Step States are "
                                 + "of different lengths. This indicates something changed "
                                 + "with the QuestInfo and the saved data is now out of sync. "
                                 + "Reset your data - as this might cause issues. QuestId: " + this.InfoSo.Id);
            }
        }

        public void IncrementQuestStepIndex()
        {
            _currentQuestStepIndex++; //this option can be higher than last quest step. Aware!
        }

        public bool IsCurrentStepExists()
        {
            return (_currentQuestStepIndex < InfoSo.questStepPrefabs.Length);
        }

        //creates quest step and adds values to it
        public void InstantiateCurrentQuestStep(Transform parentTransform)
        {
            GameObject questStepPrefab = GetCurrentQuestStepPrefab();
            if (questStepPrefab != null)
            {
                //set current active step (so we can interact with it)
                _currentQuestStepGameObject = Object.Instantiate(questStepPrefab, parentTransform);
                QuestStep questStep = _currentQuestStepGameObject.GetComponent<QuestStep>();
                
                //handle previous quest step failed situation
                if (_currentQuestStepIndex > 0)
                {
                    if (_questStepData[_currentQuestStepIndex - 1].isFailed)
                    {
                        QuestDebug.Instance.Log($"InstantiateCurrentQuestStep index:{_currentQuestStepIndex} with previous isFailed");
                        //invokes in this method in current frame before QuestStep.Start(). QuestStep.Start() invokes in next frame.
                        questStep.InitializeQuestStep(InfoSo.Id, _currentQuestStepIndex, new QuestStepData("","",true)); // set fail value if previous failed. Can be handled in current quest step.
                        
                        return;
                    }
                }
                QuestDebug.Instance.Log($"InstantiateCurrentQuestStep index:{_currentQuestStepIndex} Default data");
                questStep.InitializeQuestStep(InfoSo.Id, _currentQuestStepIndex, _questStepData[_currentQuestStepIndex]); //_questStepData[_currentQuestStepIndex] for load needs
            }

        }

        [CanBeNull]
        public GameObject GetCurrentQuestStepGameObject()
        {
            return _currentQuestStepGameObject;
        }

        private GameObject GetCurrentQuestStepPrefab()
        {
            GameObject questStepPrefab = null;
            if (IsCurrentStepExists())
            {
                questStepPrefab = InfoSo.questStepPrefabs[_currentQuestStepIndex];
            }
            else
            {
                Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                                 + "there's no current step: QuestId=" + InfoSo.Id + ", stepIndex=" + _currentQuestStepIndex);
            }
            return questStepPrefab;
        }
    
        public void StoreQuestStepValues(QuestStepData questStepData, int stepIndex)
        {
            if (stepIndex < _questStepData.Length)
            {
                _questStepData[stepIndex].stepProgress = questStepData.stepProgress;
                _questStepData[stepIndex].stepObjective = questStepData.stepObjective;
                _questStepData[stepIndex].isFailed = questStepData.isFailed;
            
            }
            else 
            {
                Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: "
                                 + "Quest Id = " + InfoSo.Id + ", Step Index = " + stepIndex);
            }
        }
    
        public QuestData GetQuestData()
        {
            return new QuestData(StateEnum, _currentQuestStepIndex, _questStepData);
        }
        
        public int GetCurrentStepIndex()
        {
            return _currentQuestStepIndex;
        }
    
        //For UI - get quest step values (from previous and current) and print it in UI.
        public string GetFullStepsData()
        {
            string fullStatus = "";
            

            //if no reqs reached and quest not started -> no status info returns
            if (StateEnum == QuestStateEnum.RequirementsNotMet || StateEnum == QuestStateEnum.CanStart)
            {
                return String.Empty;
            }

            // display all previous steps  with colors
            bool isSomeOfPreviousStepIsFailed = false;
            for (int i = 0; i < _currentQuestStepIndex; i++)
            {
                if (!_questStepData[i].isFailed){
                    if (i == InfoSo.questStepPrefabs.Length-1) //handle step state display for last step
                    {
                        fullStatus += "<color=green><b>" + _questStepData[i].stepObjective+ "</b></color>\n";
                        fullStatus += "<color=green><i>" + _questStepData[i].stepProgress+ "</i></color>\n";
                    }
                    else
                    {
                        fullStatus += "<color=green><b>" + _questStepData[i].stepObjective+ "</b></color>\n";
                    }
                }
                else if(!isSomeOfPreviousStepIsFailed)
                {
                    fullStatus += "<color=red><b>" + _questStepData[i].stepObjective+ "</b></color>\n";
                    isSomeOfPreviousStepIsFailed = true;
                }
                else
                {
                    //this empty space hides auto fail option from step (InitializeQuestStepData)
                }
                
                
            }
            
            // display the current step, if previous not failed (to handle cascade fail)
            if (IsCurrentStepExists()&&!isSomeOfPreviousStepIsFailed) 
            {

                fullStatus += "<color=yellow><b>" + _questStepData[_currentQuestStepIndex].stepObjective+ "</b></color>\n";
                fullStatus += "<color=yellow><i>" + _questStepData[_currentQuestStepIndex].stepProgress+ "</i></color>\n";

            }
            else
            {
               //if previous fails - don't print step states for current step in ui
            }


            return fullStatus;
        }

    }
}
