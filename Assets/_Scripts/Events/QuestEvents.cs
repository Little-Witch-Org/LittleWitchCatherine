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
        
        public event Action<Quest> OnQuestStateChanged;
        public void QuestStateChanged(Quest quest) 
        {
            OnQuestStateChanged?.Invoke(quest);
        }
        
        public event Action<string, int, QuestStepData> OnQuestStepDataChange;
        public void ChangeQuestStepData(string id, int stepIndex, QuestStepData questStepData) 
        {
            OnQuestStepDataChange?.Invoke(id, stepIndex, questStepData);
        }
        
        public event Func<QuestInfoSo,Quest> OnQuestByQuestInfoSoRequest; //todo change other events using this style
        public Quest RequestQuestByQuestInfoSo(QuestInfoSo questSo) 
        {
            return OnQuestByQuestInfoSoRequest?.Invoke(questSo);
        }
        
        public event Func<string,Quest> OnQuestByQuestIdRequest;
        public Quest RequestQuestByQuestId(string questId) 
        {
            return OnQuestByQuestIdRequest?.Invoke(questId);
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
        public void SetQuestAvailability(string questSoId, bool isAvailable) 
        {
            OnQuestAvailabilityChange?.Invoke(questSoId, isAvailable);
        }
        
        public event Action<string,bool> OnQuestVisibilityChange;
        public void SetQuestVisibility(string questSoId, bool isAvailable) 
        {
            OnQuestVisibilityChange?.Invoke(questSoId, isAvailable);
        }
        
        public event Action OnUpdateQuestVisibilityInUI; //updates quest visibility in quest log
        public void UpdateQuestVisibilityInUI() 
        {
            OnUpdateQuestVisibilityInUI?.Invoke();
        }
        
        public event Action<string,bool> OnFinishCurrentQuestStep;
        public void FinishCurrentQuestStep(string questId, bool isFailed) 
        {
            OnFinishCurrentQuestStep?.Invoke(questId, isFailed);
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