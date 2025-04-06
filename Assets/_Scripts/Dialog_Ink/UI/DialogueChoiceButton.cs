using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.Dialog_Ink.UI
{
    public class DialogueChoiceButton : MonoBehaviour, ISelectHandler
    {
        [Header("Components")]
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text choiceText;
        
        private int _choiceIndex  =-1;

        public void SetChoiceText(string choiceTextString)
        {
            choiceText.text = choiceTextString;
        }

        public void SetChoiceIndex(int choiceIndex)
        {
            _choiceIndex = choiceIndex;
        }

        public void SelectButton()
        {
            button.Select();
        }


        public void OnSelect(BaseEventData eventData)
        {
            EventManager.Instance.DialogueEvents.UpdateChoiceIndex(_choiceIndex);
        }
    }
    
    
}