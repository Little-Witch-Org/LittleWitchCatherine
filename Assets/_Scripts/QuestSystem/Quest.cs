using UnityEngine;


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
            _currentQuestStepIndex++;
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
                Debug.Log("InstantiateCurrentQuestStep -  set default value");
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
    
        //For UI - get quest step statuses (from previous and current) and print it in UI. Also print quest status //todo need to separate it or disable quest status
        public string GetFullStatusText()
        {
            string fullStatus = "";

            if (StateEnum == QuestStateEnum.RequirementsNotMet)
            {
                fullStatus = "Требования для старта квеста не выполнены";
            }
            else if (StateEnum == QuestStateEnum.CanStart)
            {
                fullStatus = "Квест можно начать.";
            }
            else 
            {
                // display all previous quests with strikethrough
                for (int i = 0; i < _currentQuestStepIndex; i++)
                {
                    fullStatus += "<s>" + _questStepInfoValues[i].status + "</s>\n";
                }
                // display the current step, if it exists
                if (IsCurrentStepExists())
                {
                    fullStatus += _questStepInfoValues[_currentQuestStepIndex].status;
                }
                // when the quest is completed or turned in
                if (StateEnum == QuestStateEnum.CanFinish)
                {
                    fullStatus += "Квест можно завершить!";
                }
                else if (StateEnum == QuestStateEnum.Finished)
                {
                    fullStatus += "Квест выполнен";
                }
            }

            return fullStatus;
        }
    }
}
