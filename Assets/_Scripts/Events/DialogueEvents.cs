using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace _Scripts.Events
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

        //----update story variable----
        // Базовое событие
        public event Action<string, string, Ink.Runtime.Object> OnUpdateInkDialogueVariable;

        // overload (also can be implemented using 'object value' and switch case int/bool/etc,,,
        public void UpdateInkDialogueVariable(string storyName, string varName, string value)
        {
            OnUpdateInkDialogueVariable?.Invoke(storyName, varName, new StringValue(value));
        }

        public void UpdateInkDialogueVariable(string storyName, string varName, int value)
        {
            OnUpdateInkDialogueVariable?.Invoke(storyName, varName, new IntValue(value));
        }

        public void UpdateInkDialogueVariable(string storyName, string varName, float value)
        {
            OnUpdateInkDialogueVariable?.Invoke(storyName, varName, new FloatValue(value));
        }

        public void UpdateInkDialogueVariable(string storyName, string varName, bool value)
        {
            OnUpdateInkDialogueVariable?.Invoke(storyName, varName, new BoolValue(value));
        }

        // null
        public void UpdateInkDialogueVariable(string storyName, string varName)
        {
            OnUpdateInkDialogueVariable?.Invoke(storyName, varName, new StringValue(null));
        }
        
        //---------------
        
        public event Action<string, string> OnCompleteDialogueKnot;
        public void CompleteDialogueKnot(string characterName, string dialogueKnotName)
        {
            OnCompleteDialogueKnot?.Invoke(characterName, dialogueKnotName);
        }
        
        public event Action OnSkipTypingText;
        public void SkipTypingText()
        {
            OnSkipTypingText?.Invoke();
        }
        
        public event Action OnChoiceButtonsAppears;
        public void ChoiceButtonsAppears()
        {
            OnChoiceButtonsAppears?.Invoke();
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
        
        public event Action<string, string, bool> OnUpdateDialogueStatesFromUI;
        public void UpdateDialogueStatesFromUI(string npcName, string dialogueName , bool isChecked)
        {
            OnUpdateDialogueStatesFromUI?.Invoke(npcName, dialogueName, isChecked);
        }
        
        public event Action<string, string> OnSetCustomDialogueKnot; //can be cleaned by set = ""
        public void SetCustomDialogueKnot(string npcName, string customKnotName)
        {
            OnSetCustomDialogueKnot?.Invoke(npcName, customKnotName);
        }
        
        public event Action OnSyncVariables;
        public void SyncVariables()
        {
            OnSyncVariables?.Invoke();
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