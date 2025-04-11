using System;

namespace _Scripts.test.QuestSystem
{
    /// <summary>
    /// Event class for quests //todo make it part of event system in dev (now it in test). Need to create event manager etc
    /// </summary>
    public class QuestEvents_Test
    {
        public event Action<string> OnStartQuest;
        public void StartQuest(string id) 
        {
            OnStartQuest?.Invoke(id);
        }
        
        public event Action<string> OnAdvanceQuest;
        public void AdvanceQuest(string id) 
        {
            OnAdvanceQuest?.Invoke(id);
        }
        
        public event Action<string> OnFinishQuest;
        public void FinishQuest(string id) 
        {
            OnFinishQuest?.Invoke(id);
        }
        
        public event Action<Quest_Test> OnQuestStateChange;
        public void QuestStateChange(Quest_Test questTest) 
        {
            OnQuestStateChange?.Invoke(questTest);
        }
        
        public event Action<string, int, QuestStepState_Test> OnQuestStepStateChange;
        public void QuestStepStateChange(string id, int stepIndex, QuestStepState_Test questStepStateTest) 
        {
            OnQuestStepStateChange?.Invoke(id, stepIndex, questStepStateTest);
        }
        
    }
}