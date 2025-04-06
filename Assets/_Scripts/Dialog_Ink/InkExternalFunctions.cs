using Ink.Runtime;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{
    public class InkExternalFunctions
    {
        //bind ink external function with class methods
        public void Bind(Story story)
        {
            story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
            story.BindExternalFunction("AdvanceQuest", (string questId) => AdvanceQuest(questId));
            story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
        }
        
        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("StartQuest");
            story.UnbindExternalFunction("AdvanceQuest");
            story.UnbindExternalFunction("FinishQuest");
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
    }
}