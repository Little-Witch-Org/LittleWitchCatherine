using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace _Scripts.Dialog_Ink
{
    public class DialogueEvents
    {
        public Action<string> OnEnterDialogue;
        //this method used to manually trigger event (as auto invoke in some places in start or update)
        public void EnterDialogue(string knotName)
        {
            //same as OnEnterDialogue?.Invoke(knotName)
            if (OnEnterDialogue != null)
            {
                OnEnterDialogue(knotName);
            }
        }
        
        public Action OnDialogueStarted;
        public void DialogueStarted()
        {
            OnDialogueStarted?.Invoke();
        }
        
        public Action OnDialogueFinished;
        public void DialogueFinished()
        {
            OnDialogueFinished?.Invoke();
        }

        public Action<string,List<Choice>> OnDisplayDialogue;

        public void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
        {
            OnDisplayDialogue?.Invoke(dialogueLine,dialogueChoices);
        }

        public Action<int> OnUpdateChoiceIndex;

        public void UpdateChoiceIndex(int choiceIndex)
        {
            OnUpdateChoiceIndex?.Invoke(choiceIndex);
        }

        public event Action<string, Ink.Runtime.Object> OnUpdateInkDialogueVariable;

        public void UpdateInkDialogueVariable(string name, Ink.Runtime.Object value)
        {
            OnUpdateInkDialogueVariable?.Invoke(name,value);
        }
    }
    
}