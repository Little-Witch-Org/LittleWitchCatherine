using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{
    public class DialogueEvents
    {
        public Action<string, string, bool> OnEnterDialogue;
        //this method used to manually trigger event (as auto invoke in some places in start or update)
        public void EnterDialogue(string storyName, string knotName, bool isCutsceneUI)
        {
            OnEnterDialogue?.Invoke(storyName, knotName, isCutsceneUI);
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
        
        public Action OnDialogueStartedCutscene;
        public void DialogueStartedCutscene()
        {
            OnDialogueStartedCutscene?.Invoke();
        }
        
        public Action OnDialogueFinishedCutscene;
        public void DialogueFinishedCutscene()
        {
            OnDialogueFinishedCutscene?.Invoke();
        }

        public Action<string,List<Choice>,bool> OnDisplayDialogue;

        public void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices,bool isCutsceneUI)
        {
            OnDisplayDialogue?.Invoke(dialogueLine,dialogueChoices,isCutsceneUI);
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
        
        
        //Utils
        public event Action<string> OnStartDialogueWithNpc;
        public void StartDialogueWithNpc(string npcName)
        {
            OnStartDialogueWithNpc?.Invoke(npcName);
        }
        
        public event Action<string, bool> OnSetDialogueAutoActivation;
        public void SetDialogueAutoActivation(string npcName, bool isActive)
        {
            OnSetDialogueAutoActivation?.Invoke(npcName, isActive);
        }
        
        
        
        //tags
        public event Action<string> OnTagChangeSpeaker1Name;
        public void TagChangeSpeaker1Name(string speaker1NameTag)
        {
            OnTagChangeSpeaker1Name?.Invoke(speaker1NameTag);
        }
        
        public event Action<string> OnTagChangeSpeaker2Name;
        public void TagChangeSpeaker2Name(string speaker2NameTag)
        {
            OnTagChangeSpeaker2Name?.Invoke(speaker2NameTag);
        }
        
        public event Action<string> OnTagChangePortrait1;
        public void TagChangePortrait1(string portraitTag1)
        {
            OnTagChangePortrait1?.Invoke(portraitTag1);
        }
        public event Action<string> OnTagChangePortrait2;
        public void TagChangePortrait2(string portraitTag2)
        {
            OnTagChangePortrait2?.Invoke(portraitTag2);
        }
        
        public event Action<string> OnTagChangeCurrentSpeaker;
        public void TagChangeCurrentSpeaker(string currentSpeaker)
        {
            OnTagChangeCurrentSpeaker?.Invoke(currentSpeaker);
        }
    }
    
}