using System;
using _Scripts.QuestSystem;
using UnityEngine;

namespace _Scripts.Events
{
    /// <summary>
    /// Event class for quests
    /// </summary>
    public class QuestEvents
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
        
        public event Action<Quest> OnQuestStateChange;
        public void QuestStateChange(Quest quest) 
        {
            OnQuestStateChange?.Invoke(quest);
        }
        
        public event Action<string, int, QuestStepValues> OnQuestStepValuesChange;
        public void QuestStepValuesChange(string id, int stepIndex, QuestStepValues questStepValues) 
        {
            OnQuestStepValuesChange?.Invoke(id, stepIndex, questStepValues);
        }
        
        public event Func<QuestInfoSo,Quest> OnRequestQuestByQuestInfoSo;
        public Quest RequestQuestByQuestInfoSo(QuestInfoSo questSo) 
        {
            return OnRequestQuestByQuestInfoSo?.Invoke(questSo);
        }
        
        public event Action<GameObject> OnQuestStepCreated;
        public void QuestStepCreated(GameObject questStep) 
        {
            OnQuestStepCreated?.Invoke(questStep);
        }
        
        public event Action<GameObject> OnQuestStepDeleted;
        public void QuestStepDeleted(GameObject questStep) 
        {
            OnQuestStepDeleted?.Invoke(questStep);
        }
        
        public event Action<string,bool> OnQuestAvailabilityChange;
        public void QuestAvailabilityChange(string questSoId, bool isAvailable) 
        {
            OnQuestAvailabilityChange?.Invoke(questSoId, isAvailable);
        }

        


        //custom quest step events 
        public event Action<string, bool> OnTriggerFirstDevQuestStep1;
        public void TriggerFirstDevQuestStep1(string customParam, bool isFailed) 
        {
            OnTriggerFirstDevQuestStep1?.Invoke(customParam, isFailed);
        }
        
        public event Action<string, bool> OnTriggerSecondDevQuestStep1;
        public void TriggerSecondDevQuestStep1(string customParam, bool isFailed) 
        {
            OnTriggerSecondDevQuestStep1?.Invoke(customParam, isFailed);
        }
        
        public event Action<string, bool> OnTriggerSecondDevQuestStep2;
        public void TriggerSecondDevQuestStep2(string customParam, bool isFailed) 
        {
            OnTriggerSecondDevQuestStep2?.Invoke(customParam, isFailed);
        }
        
        public event Action<string, bool> OnTriggerQuest1GoDownToKitchenStep1;
        public void TriggerQuest1GoDownToKitchenStep1(string customParam, bool isFailed) 
        {
            OnTriggerQuest1GoDownToKitchenStep1?.Invoke(customParam, isFailed);
        }

    }
}