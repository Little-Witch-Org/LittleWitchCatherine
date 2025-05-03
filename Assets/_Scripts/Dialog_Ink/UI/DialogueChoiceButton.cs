using _Scripts.Managers;
using _Scripts.Service.Log;
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
        

        //if button selected (by mouse) and we click (use submit event) choice index updates fore dialogue and submit method continues dialogue.
        public void OnSelect(BaseEventData eventData)
        {
            //DialogDebug.Instance.Log("DialogueChoiceButton.OnSelect choice "+ _choiceIndex);
            EventManager.Instance.DialogueEvents.UpdateChoiceIndex(_choiceIndex);
        }
    }
    
    
}