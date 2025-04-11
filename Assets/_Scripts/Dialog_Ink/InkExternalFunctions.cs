using Ink.Runtime;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{
    public class InkExternalFunctions
    {
        //bind ink external function with class methods //todo do we need to handle all type of methods ? 
        //todo We can bind all ext funk to all stories. But it will be better to add .ink parser and bind only declared.
        //TODO WE CAN TRY TO USE TAGS (from other video) instead of ext. functions (for ability to play in editor)
        public void Bind(Story story)
        {
            story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
            story.BindExternalFunction("AdvanceQuest", (string questId) => AdvanceQuest(questId));
            story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
            story.BindExternalFunction("CompleteDialogueKnot", (string characterName, string dialogueKnotName) => CompleteDialogueKnot(characterName, dialogueKnotName));
        }
        
        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("StartQuest");
            story.UnbindExternalFunction("AdvanceQuest");
            story.UnbindExternalFunction("FinishQuest");
            story.UnbindExternalFunction("CompleteDialogueKnot");
        }
        
        
        
        
        private void StartQuest(string questId)
        {
            EventManager.Instance.QuestEvents.StartQuest(questId);
        }
        
        private void AdvanceQuest(string questId)
        {
            EventManager.Instance.QuestEvents.AdvanceQuest(questId);
        }
        
        private void FinishQuest(string questId)
        {
            EventManager.Instance.QuestEvents.FinishQuest(questId);
        }
        
        private void CompleteDialogueKnot(string characterName, string dialogueKnotName)
        {
            EventManager.Instance.DialogueEvents.CompleteDialogueKnot(characterName, dialogueKnotName);
        }
    }
}