using _Scripts.test.QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestLogButton : MonoBehaviour, ISelectHandler
{
    public Button button{get; private set;}
    private TMP_Text buttonText;
    private UnityAction OnSelectAction;


    //need to initialize manually
    public void Initialize(string displayName, UnityAction sendAction)
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>(); //assign using code (not in inspector)
        
        buttonText.text = displayName;
        OnSelectAction = sendAction;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        OnSelectAction();
    }
    
    public void SetState(QuestStateEnum_Test stateEnum)
    {
        switch (stateEnum)
        {
            case QuestStateEnum_Test.RequirementsNotMet:
            case QuestStateEnum_Test.CanStart:
                buttonText.color = Color.red;
                break;
            case QuestStateEnum_Test.InProgress:
            case QuestStateEnum_Test.CanFinish:
                buttonText.color = Color.yellow;
                break;
            case QuestStateEnum_Test.Finished:
                buttonText.color = Color.green;
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement: " + stateEnum);
                break;
        }
    }
}
