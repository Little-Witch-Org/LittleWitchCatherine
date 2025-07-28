using _Scripts.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.QuestSystem.UI
{
    public class QuestLogButton : MonoBehaviour, ISelectHandler
    {
        public Button Button{get; private set;}
        private TMP_Text _questNameText;
        private UnityAction _onSelectAction;


        //need to initialize manually
        public void Initialize(string displayName, UnityAction sendAction)
        {
            Button = GetComponent<Button>();
            _questNameText = GetComponentInChildren<TMP_Text>(); //assign using code (not in inspector)
        
            _questNameText.text = displayName;
            _onSelectAction = sendAction;
        }
    
        public void OnSelect(BaseEventData eventData)
        {
            _onSelectAction();
        }
    
        public void SetState(QuestStateEnum stateEnum)
        {
            switch (stateEnum)
            {
                case QuestStateEnum.RequirementsNotMet:
                    _questNameText.color = Color.grey;
                    break;
                case QuestStateEnum.CanStart:
                    _questNameText.color = Color.white;
                    break;
                case QuestStateEnum.InProgress:
                    _questNameText.color = Color.yellow;
                    break;
                case QuestStateEnum.CanFinish:
                    _questNameText.color = Color.yellow;
                    break;
                case QuestStateEnum.Finished:
                    _questNameText.color = Color.green;
                    break;
                case QuestStateEnum.Failed:
                    _questNameText.color = Color.red;
                    break;
                default:
                    Debug.LogWarning("Quest State not recognized by switch statement: " + stateEnum);
                    break;
            }
        }

        public string GetButtonText()
        {
            return _questNameText.text;
        }
    }
}
