using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{
    public class DialogueEvents
    {
        public Action<string, string> OnEnterDialogue;
        //this method used to manually trigger event (as auto invoke in some places in start or update)
        public void EnterDialogue(string storyName, string knotName)
        {
            OnEnterDialogue?.Invoke(storyName, knotName);
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

        public event Action<string, string, Ink.Runtime.Object> OnUpdateInkDialogueVariable;

        public void UpdateInkDialogueVariable(string storyName, string varName, Ink.Runtime.Object varValue)
        {
            OnUpdateInkDialogueVariable?.Invoke(storyName, varName, varValue);
        }
        
        public event Action<string, string> OnCompleteDialogueKnot;
        public void CompleteDialogueKnot(string characterName, string dialogueKnotName)
        {
            OnCompleteDialogueKnot?.Invoke(characterName, dialogueKnotName);
        }
    }
    
}