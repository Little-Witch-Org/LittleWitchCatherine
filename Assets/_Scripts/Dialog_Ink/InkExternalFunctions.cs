using _Scripts.Managers;
using Ink.Runtime;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{
    public class InkExternalFunctions
    {
        //bind ink external function with class methods //todo do we need to handle all type of methods ? 
        //todo We can bind all ext funk to all stories. But it will be better to add .ink parser and bind only declared.
        public void Bind(Story story)
        {
            story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
            story.BindExternalFunction("AdvanceQuest", (string questId) => AdvanceQuest(questId));
            story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
            story.BindExternalFunction("CompleteDialogueKnot", (string characterName, string dialogueKnotName) => CompleteDialogueKnot(characterName, dialogueKnotName));
            story.BindExternalFunction("LaunchCutscene", (string cutsceneId) => LaunchCutscene(cutsceneId));
            story.BindExternalFunction("ResumeCutscene", () => ResumeCutscene());
            
            story.BindExternalFunction("AddMinutes",  (int minutes) =>
            {
                //Debug.Log($"AddMinutes called with: {minutes}");
                AddMinutes(minutes);
            });
            
            //stats
            story.BindExternalFunction("UpdateHealth", (int health) => UpdateHealth(health));
            story.BindExternalFunction("UpdateSatiety", (int Satiety) => UpdateSatiety(Satiety));
            story.BindExternalFunction("UpdateMood", (int mood) => UpdateMood(mood));
            story.BindExternalFunction("UpdateEnergy", (int energy) => UpdateEnergy(energy));
            
            //rep
            story.BindExternalFunction("UpdateReputation", (string npcName, int reputation) => UpdateReputation(npcName,reputation));
            
        }
        
        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("StartQuest");
            story.UnbindExternalFunction("AdvanceQuest");
            story.UnbindExternalFunction("FinishQuest");
            story.UnbindExternalFunction("CompleteDialogueKnot");
            story.UnbindExternalFunction("LaunchCutscene");
            story.UnbindExternalFunction("ResumeCutscene");
            
            story.UnbindExternalFunction("AddMinutes");
            
            
            story.UnbindExternalFunction("UpdateHealth");
            story.UnbindExternalFunction("UpdateSatiety");
            story.UnbindExternalFunction("UpdateMood");
            story.UnbindExternalFunction("UpdateEnergy");
            
            story.UnbindExternalFunction("UpdateReputation");
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
        
        private void LaunchCutscene( string cutsceneId)
        {
            EventManager.Instance.CutsceneEvents.LaunchCutscene(cutsceneId);
        }

        private void ResumeCutscene()
        {
            EventManager.Instance.CutsceneEvents.ResumeCutscene();
        }
        
        private void AddMinutes(int minutes)
        {
            TimeManager.Instance.AddMinutes(minutes);
        }
        
        //change player stats
        private void UpdateHealth(int health)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateHealth(health);
        }
        private void UpdateSatiety(int satiety)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateSatiety(satiety);
        }
        private void UpdateMood(int mood)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateMood(mood);
        }
        private void UpdateEnergy(int energy)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateEnergy(energy);
        }
        
        //reputation
        private void UpdateReputation(string npcName, int reputation)
        {
            EventManager.Instance.ReputationEvents.UpdateReputation(npcName, reputation);
        }
    }
}