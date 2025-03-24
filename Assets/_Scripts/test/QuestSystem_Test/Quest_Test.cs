using _Scripts.test.QuestSystem;
using UnityEngine;

public class Quest_Test
{
    //static info
    public QuestInfoSo_Test info;
    
    //stateTest info
    public QuestState_Test StateTest;
    
    private int _currentQuestStepIndex;
    
    private QuestStepState_Test[] _questStepStates;

    public Quest_Test(QuestInfoSo_Test questInfo)
    {
        this.info = questInfo;
        this.StateTest = QuestState_Test.RequirementsNotMet;
        this._currentQuestStepIndex = 0;
        this._questStepStates = new QuestStepState_Test[info.questStepPrefabs.Length];

        for (int i = 0; i < _questStepStates.Length; i++)//initialising states for every quest steps
        {
            _questStepStates[i] = new QuestStepState_Test();
        }
    }
    
    //for load needs
    public Quest_Test(QuestInfoSo_Test questInfo, QuestState_Test questStateTest, int currentQuestStepIndex, QuestStepState_Test[] questStepStates)
    {
        this.info = questInfo;
        this.StateTest = questStateTest;
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
            
        }
        else 
        {
            Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: "
                             + "Quest_Test Id = " + info.Id + ", Step Index = " + stepIndex);
        }
    }
    
    public QuestData_Test GetQuestData()
    {
        return new QuestData_Test(StateTest, _currentQuestStepIndex, _questStepStates);
    }
}
