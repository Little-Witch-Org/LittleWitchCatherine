using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.CheatMenu
{
    public class DialogItemUI : MonoBehaviour
    {

        [SerializeField] private TMP_Text dialogueKnotName;
        [SerializeField] private Toggle isDialogueKnotComplete;
        

        public bool isExternalUpdate;
        
        public Action<string, bool> OnCheckboxValueChanged;
        
        private void Awake()
        {
            isDialogueKnotComplete.onValueChanged.AddListener(HandleToggleChanged); //subscribe on toggle event
        }
        private void OnDestroy()
        {
            isDialogueKnotComplete.onValueChanged.RemoveListener(HandleToggleChanged);
        }
        
        public string DialogueName
        {
            get => dialogueKnotName.text;
            set => dialogueKnotName.text = value;
        }
        
        public bool IsCompleted
        {
            get => isDialogueKnotComplete.isOn;
            set => isDialogueKnotComplete.isOn = value;
        }

        public void UpdateCompleteStateExternal(bool isComplete)
        {
            if (IsCompleted == isComplete)
            {
                //Debug.Log("external return in item cause non change");
                return;
            }

            //Debug.Log("external change state in item");
            isExternalUpdate = true;
            isDialogueKnotComplete.isOn = isComplete;
            
        }

        private void HandleToggleChanged(bool isOn)
        {
            if (isExternalUpdate)
            {
                //Debug.Log("toggle from external");
                isExternalUpdate = false;
                return;
            }
            
            //Debug.Log($"toggle with {dialogueKnotName.text} is {isOn}");
            OnCheckboxValueChanged?.Invoke(DialogueName, isOn);
        }


    }
}