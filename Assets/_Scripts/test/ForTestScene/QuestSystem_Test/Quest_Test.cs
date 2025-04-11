using _Scripts.test.QuestSystem;
using UnityEngine;

public class Quest_Test
{
    //static info
    public QuestInfoSo_Test info;
    
    //stateEnumTest info
    public QuestStateEnum_Test StateEnumTest;
    
    private int _currentQuestStepIndex;
    
    private QuestStepState_Test[] _questStepStates;

    public Quest_Test(QuestInfoSo_Test questInfo)
    {
        this.info = questInfo;
        this.StateEnumTest = QuestStateEnum_Test.RequirementsNotMet;
        this._currentQuestStepIndex = 0;
        this._questStepStates = new QuestStepState_Test[info.questStepPrefabs.Length];

        for (int i = 0; i < _questStepStates.Length; i++)//initialising states for every quest steps
        {
            _questStepStates[i] = new QuestStepState_Test();
        }
    }
    
    //for load needs
    public Quest_Test(QuestInfoSo_Test questInfo, QuestStateEnum_Test questStateEnumTest, int currentQuestStepIndex, QuestStepState_Test[] questStepStates)
    {
        this.info = questInfo;
        this.StateEnumTest = questStateEnumTest;
        this._currentQuestStepIndex = currentQuestStepIndex;
        this._questStepStates = questStepStates;

        // if the quest step states and prefabs are different lengths,
        // something has changed during development and the saved data is out of sync.
        if (this._questStepStates.Length != this.info.questStepPrefabs.Length)
        {
            Debug.LogWarning("Quest_Test Step Prefabs and Quest_Test Step States are "
                             + "of different lengths. This indicates something changed "
                             + "with the QuestInfo and the saved data is now out of sync. "
                             + "Reset your data - as this might cause issues. QuestId: " + this.info.Id);
        }
    }

    public void MoveToNextStep()
    {
        _currentQuestStepIndex++;
    }

    public bool IsCurrentStepExists()
    {
        return (_currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep_Test questStepTest = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep_Test>();
            questStepTest.InitializeQuestStep(info.Id, _currentQuestStepIndex, _questStepStates[_currentQuestStepIndex].state);
        }

    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (IsCurrentStepExists())
        {
            questStepPrefab = info.questStepPrefabs[_currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                             + "there's no current step: QuestId=" + info.Id + ", stepIndex=" + _currentQuestStepIndex);
        }
        return questStepPrefab;
    }
    
    public void StoreQuestStepState(QuestStepState_Test questStepStateTest, int stepIndex)
    {
        if (stepIndex < _questStepStates.Length)
        {
            _questStepStates[stepIndex].state = questStepStateTest.state;
            _questStepStates[stepIndex].status = questStepStateTest.status;
            
        }
        else 
        {
            Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: "
                             + "Quest_Test Id = " + info.Id + ", Step Index = " + stepIndex);
        }
    }
    
    public QuestData_Test GetQuestData()
    {
        return new QuestData_Test(StateEnumTest, _currentQuestStepIndex, _questStepStates);
    }
    
    public string GetFullStatusText()
    {
        string fullStatus = "";

        if (StateEnumTest == QuestStateEnum_Test.RequirementsNotMet)
        {
            fullStatus = "Requirements are not yet met to start this quest.";
        }
        else if (StateEnumTest == QuestStateEnum_Test.CanStart)
        {
            fullStatus = "This quest can be started!";
        }
        else 
        {
            // display all previous quests with strikethroughs
            for (int i = 0; i < _currentQuestStepIndex; i++)
            {
                fullStatus += "<s>" + _questStepStates[i].status + "</s>\n";
            }
            // display the current step, if it exists
            if (IsCurrentStepExists())
            {
                fullStatus += _questStepStates[_currentQuestStepIndex].status;
            }
            // when the quest is completed or turned in
            if (StateEnumTest == QuestStateEnum_Test.CanFinish)
            {
                fullStatus += "The quest is ready to be turned in.";
            }
            else if (StateEnumTest == QuestStateEnum_Test.Finished)
            {
                fullStatus += "The quest has been completed!";
            }
        }

        return fullStatus;
    }
}
