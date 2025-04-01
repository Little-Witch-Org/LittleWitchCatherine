using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.QuestSystem.UI
{
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
    
        public void SetState(QuestStateEnum stateEnum)
        {
            switch (stateEnum)
            {
                case QuestStateEnum.RequirementsNotMet:
                    buttonText.color = Color.grey;
                    break;
                case QuestStateEnum.CanStart:
                    buttonText.color = Color.white;
                    break;
                case QuestStateEnum.InProgress:
                    buttonText.color = Color.yellow;
                    break;
                case QuestStateEnum.CanFinish:
                    buttonText.color = Color.yellow;
                    break;
                case QuestStateEnum.Finished:
                    buttonText.color = Color.green;
                    break;
                case QuestStateEnum.Failed:
                    buttonText.color = Color.red;
                    break;
                default:
                    Debug.LogWarning("Quest State not recognized by switch statement: " + stateEnum);
                    break;
            }
        }
    }
}
