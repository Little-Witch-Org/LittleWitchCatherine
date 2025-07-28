using System;
using System.Reflection;
using _Scripts.Events;
using _Scripts.Managers;
using _Scripts.Service.Log;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.QuestSystem
{
    /// <summary>
    /// Structure:
    /// Initialization (in quest class)
    /// Start and subscribe on trigger if exist
    /// Invokes ActivateSubscribedMethodOnTrigger if triggers
    /// FinishQuesStep. Can be invoked internal or external.
    /// InvokesOnFinishQuestStep invokes by FinishQuesStep.
    /// Advance quest.
    /// InvokesAfterAdvanceQuest. Can handle self finish.
    /// </summary>
    public abstract class QuestStep : MonoBehaviour //todo separate objectives and status to prepare for quest cheat menu + localization
    {
        private Delegate _subscribedHandler; // save delegate to unsubscribe
    
        private bool _isFinished;
        
        private string _questId;

        private int _stepIndex; //first step 0, second 1 etc

        protected bool IsPreviousFailed = false; //deprecated ? 
        
        [SerializeField] protected bool failIfPreviousFailed;  //can be used for measure the progress

        [SerializeField] protected bool isNoTriggersForStep; //if no triggers needed for this step (step can be finished by dialogue etc) we can check this option.

        
        protected virtual void Start()
        {
            if (!isNoTriggersForStep)
            {
                AutoSubscribeOnStepTriggerEvent();
            }

            EventManager.Instance.QuestEvents.QuestStepCreated(gameObject);
            QuestDebug.Instance.Log($"Quest step created: {_questId} step {_stepIndex+1}");
        }

        private void OnDisable()
        {
            if (!isNoTriggersForStep)
            {
                AutoUnsubscribeFromStepTriggerEvent();
            }

            EventManager.Instance.QuestEvents.QuestStepDeleted(gameObject);
        }


        //Used to initialize this step by Quest class. Invokes before start in inherited quest step class.
        public void InitializeQuestStep(string questId, int stepIndex, QuestStepData questStepData)
        {
            _questId = questId;
            _stepIndex = stepIndex;
            
            InitializeQuestStepData(questStepData); //uses for loading/isfailed needs
        }
        
        //Can be used from external by events (in dialogues/other quests etc..) to finish step if we don't have triggers for current step.
        public void FinishQuesStep(bool isFailed)
        {
            if (!_isFinished)
            {
                _isFinished = true;

                InvokesOnFinishQuestStep(isFailed);
            
                EventManager.Instance.QuestEvents.AdvanceQuest(_questId);
                
                InvokesAfterAdvanceQuest();
            
                QuestDebug.Instance.Log($"All invocations in {_questId} step {_stepIndex+1} was finished, destroying..");
                
                Destroy(gameObject);
            }
            else
            {
                DialogDebug.Instance.LogError($"The quest step {_questId} step {_stepIndex+1} has already been finished.");
            }
        }
        
        //update "QuestStepData[] _questStepData" in current quest by manager (update by progress (for counting entities), objective(description), isFailed.
        protected void ChangeStepData(string newStepProgress, string newStepObjective, bool newIsFailed)
        {
            EventManager.Instance.QuestEvents
                .ChangeQuestStepData(_questId, _stepIndex, new QuestStepData(newStepProgress, newStepObjective, newIsFailed));
        }
        

        //finds in quest events class trigger event depending on step name and subscribe on it
        private void AutoSubscribeOnStepTriggerEvent()
        {
            string className = GetType().Name; // Например, "FirstDevQuestStep1"
            string eventName = "OnTrigger" + className; // Например, "OnTriggerFirstDevQuestStep1"

            if (EventManager.Instance?.QuestEvents == null)
            {
                QuestDebug.Instance.LogError("QuestEvents не доступен!");
                return;
            }

            EventInfo eventInfo = typeof(QuestEvents).GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);
            if (eventInfo == null)
            {
                QuestDebug.Instance.LogError($"Событие {eventName} не найдено в QuestEvents!");
                return;
            }

            // Создаём делегат с параметром bool и сохраняем его
            _subscribedHandler = Delegate.CreateDelegate(
                eventInfo.EventHandlerType,
                this,
                nameof(ActivateSubscribedMethodOnTrigger)
            );

            // Подписываемся
            eventInfo.AddEventHandler(EventManager.Instance.QuestEvents, _subscribedHandler);
            QuestDebug.Instance.Log($"Успешно подписались на {eventName}");
        }

        private void AutoUnsubscribeFromStepTriggerEvent()
        {
            if (_subscribedHandler == null || EventManager.Instance?.QuestEvents == null)
                return;

            string className = GetType().Name;
            string eventName = "OnTrigger" + className;

            EventInfo eventInfo = typeof(QuestEvents).GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);
            if (eventInfo == null)
            {
                QuestDebug.Instance.LogError($"Событие {eventName} не найдено при отписке!");
                return;
            }

            // Отписываемся
            eventInfo.RemoveEventHandler(EventManager.Instance.QuestEvents, _subscribedHandler);
            _subscribedHandler = null; // Очищаем ссылку
            QuestDebug.Instance.Log($"Успешно отписались от {eventName}");
        }
        //invokes this method after initialization in Quest class. Can be used to fill custom values for quest step or some invocations. !Do not turn into virtual. Handle it in actual step code!
        protected abstract void InitializeQuestStepData(QuestStepData questStepData);
        
        //using this method we can handle quest step progress (collect items\click on something\end conversation etc) 
        //we can pass a parameter providing branching within the quest step and fail parameter to fail step. Requires configuration of quest step trigger.
        protected abstract void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed);

        //Invokes in step finish method before Quest Advance. Can be used to update step data if FinishQuestStep triggers from external (dialogues etc...).
        protected abstract void InvokesOnFinishQuestStep(bool isFailed);

        //can be used to some invocations after quest advance (new step/finish quest/start dependent quest) 
        protected virtual void InvokesAfterAdvanceQuest()
        {
            //todo to make abstract
        }
    }
}
