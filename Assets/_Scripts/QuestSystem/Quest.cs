using System;
using _Scripts.Service.Log;
using UnityEngine;
using Object = UnityEngine.Object;


namespace _Scripts.QuestSystem
{
    /// <summary>
    ///Represents quest entity which contain QuestInfoSo and current quest status. Is contained in the dictionary of the quest manager.
    /// </summary>
    public class Quest
    {
        //static InfoSo
        public QuestInfoSo InfoSo;
    
        //stateEnum InfoSo
        public QuestStateEnum StateEnum;
    
        private int _currentQuestStepIndex;
    
        private QuestStepValues[] _questStepInfoValues;

        public Quest(QuestInfoSo questInfoSo)
        {
            this.InfoSo = questInfoSo;
            this.StateEnum = QuestStateEnum.RequirementsNotMet;
            this._currentQuestStepIndex = 0;
            this._questStepInfoValues = new QuestStepValues[InfoSo.questStepPrefabs.Length];

            for (int i = 0; i < _questStepInfoValues.Length; i++)//initialising states for every quest steps
            {
                _questStepInfoValues[i] = new QuestStepValues();
            }
        }
    
        //for load needs
        public Quest(QuestInfoSo questInfoSo, QuestStateEnum questStateEnum, int currentQuestStepIndex, QuestStepValues[] questStepInfoValues)
        {
            this.InfoSo = questInfoSo;
            this.StateEnum = questStateEnum;
            this._currentQuestStepIndex = currentQuestStepIndex;
            this._questStepInfoValues = questStepInfoValues;

            // if the quest step states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.
            if (this._questStepInfoValues.Length != this.InfoSo.questStepPrefabs.Length)
            {
                Debug.LogWarning("Quest Step Prefabs and Quest Step States are "
                                 + "of different lengths. This indicates something changed "
                                 + "with the QuestInfo and the saved data is now out of sync. "
                                 + "Reset your data - as this might cause issues. QuestId: " + this.InfoSo.Id);
            }
        }

        public void MoveToNextStep()
        {
            _currentQuestStepIndex++; //this option can be higher than last quest step. Aware!
        }

        public bool IsCurrentStepExists()
        {
            return (_currentQuestStepIndex < InfoSo.questStepPrefabs.Length);
        }

        //creates quest step and adds values to it
        public void InstantiateCurrentQuestStep(Transform parentTransform) //todo высрать инфу предыдущего степа (если он есть) а конкретно зафейлен ли он, чтобы передать текущ.
        {
            GameObject questStepPrefab = GetCurrentQuestStepPrefab();
            if (questStepPrefab != null)
            {
                QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
                
                //handle previous quest step failed situation
                if (_currentQuestStepIndex > 0)
                {
                    if (_questStepInfoValues[_currentQuestStepIndex - 1].isFailed)
                    {
                        //invokes in this method in current frame before QuestStep.Start() in frame after instantiation 
                        questStep.InitializeQuestStep(InfoSo.Id, _currentQuestStepIndex, new QuestStepValues("","",true)); // set fail value if previous failed
                        
                        return;
                    }
                }
                QuestDebug.Instance.Log("InstantiateCurrentQuestStep -  set default value");
                questStep.InitializeQuestStep(InfoSo.Id, _currentQuestStepIndex, _questStepInfoValues[_currentQuestStepIndex]); //_questStepInfoValues[_currentQuestStepIndex] for load needs
            }

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
    
        public void StoreQuestStepInfoValues(QuestStepValues questStepValues, int stepIndex)
        {
            if (stepIndex < _questStepInfoValues.Length)
            {
                _questStepInfoValues[stepIndex].state = questStepValues.state;
                _questStepInfoValues[stepIndex].status = questStepValues.status;
                _questStepInfoValues[stepIndex].isFailed = questStepValues.isFailed;
            
            }
            else 
            {
                Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: "
                                 + "Quest Id = " + InfoSo.Id + ", Step Index = " + stepIndex);
            }
        }
    
        public QuestData GetQuestData()
        {
            return new QuestData(StateEnum, _currentQuestStepIndex, _questStepInfoValues);
        }
    
        //For UI - get quest step statuses (from previous and current) and print it in UI. Also print quest status
        //todo need to separate quest status -> to get in ui in QuestStateChange (quest status will be tec info). Here in raw string we put previous quest step status
        //todo and current step status + state in next string + "-" or dot symbol. If there are auto lose condition we need to not display next auto loosed steps
        public string GetFullStatusText()
        {
            string fullStatus = "";
            

            //if no reqs reached and quest not started -> no status info returns
            if (StateEnum == QuestStateEnum.RequirementsNotMet || StateEnum == QuestStateEnum.CanStart)
            {
                return String.Empty;
            }

            // display all previous quests with colors
            bool isSomeOfPreviousStepIsFailed = false;
            for (int i = 0; i < _currentQuestStepIndex; i++)
            {
                if (!_questStepInfoValues[i].isFailed){
                    if (i == InfoSo.questStepPrefabs.Length-1) //handle step state display for last step
                    {
                        fullStatus += "<color=green><b>" + _questStepInfoValues[i].status + "</b></color>\n";
                        fullStatus += "<color=green><i>" + _questStepInfoValues[i].state + "</i></color>\n";
                    }
                    else
                    {
                        fullStatus += "<color=green><b>" + _questStepInfoValues[i].status + "</b></color>\n";
                    }
                }
                else if(!isSomeOfPreviousStepIsFailed)
                {
                    fullStatus += "<color=red><b>" + _questStepInfoValues[i].status + "</b></color>\n";
                    isSomeOfPreviousStepIsFailed = true;
                }
                else
                {
                    //this empty space hides auto fail option from step (SetQuestStepState)
                }
                
                
            }
            
            // display the current step, if previous not failed (to handle cascade fail)
            if (IsCurrentStepExists()&&!isSomeOfPreviousStepIsFailed) 
            {

                fullStatus += "<color=yellow><b>" + _questStepInfoValues[_currentQuestStepIndex].status + "</b></color>\n";
                fullStatus += "<color=yellow><i>" + _questStepInfoValues[_currentQuestStepIndex].state + "</i></color>\n";

            }


            return fullStatus;
        }

    }
}
