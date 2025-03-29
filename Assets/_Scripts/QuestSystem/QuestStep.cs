using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.QuestSystem
{
    //todo dont like QuestStep / QuestStepTrigger and QuestStepTriggerEnabler subscribe/unsubscribe methods and checks (in update). Need to rework
    //todo add manual subscription ? How to spawn trigger items on scene? if we use pre create in scene prefabs< they still there after quest complete but in deactivated state
    public abstract class QuestStep : MonoBehaviour
    {
        private Delegate _subscribedHandler; // Сохраняем делегат для отписки
    
        private bool _isFinished = false;
        
        private string _questId;

        private int _stepIndex; //first step 0, second 1 etc

        protected bool IsPreviousFailed = false;
        [SerializeField] protected bool failIfPreviousFailed;  //can be used for measure the progress

        
        protected virtual void Start()
        {
            AutoSubscribeOnStepTriggerEvent();
        }

        private void OnDisable()
        {
            AutoUnsubscribeFromStepTriggerEvent();
        }


        //need for initialization step in Quest class
        //this method invokes before start in quest step
        public void InitializeQuestStep(string questId, int stepIndex, QuestStepValues questStepValues)
        {
            _questId = questId;
            _stepIndex = stepIndex;
            
            //we can use this before start in inherited class
            SetQuestStepState(questStepValues); //uses for loading/isfailed needs
        }
        
        
        protected void FinishQuesStep()
        {
            if (!_isFinished)
            {
                _isFinished = true;
            
                EventManager.Instance.QuestEvents.AdvanceQuest(_questId);
            
                Destroy(gameObject);
            }
        }
        
        //update "QuestStepValues[] _questStepInfoValues" in current quest by manager (update by status (for counting entities), status(description), isFailed.
        protected void ChangeValues(string newState, string newStatus, bool newIsFailed)
        {
            EventManager.Instance.QuestEvents
                .QuestStepValuesChange(_questId, _stepIndex, new QuestStepValues(newState, newStatus, newIsFailed));
        }
        
       

        //finds in quest events class trigger event depending on step name and subscribe on it
        private void AutoSubscribeOnStepTriggerEvent()
        {
            string className = GetType().Name; // Например, "FirstDevQuestStep1"
            string eventName = "OnTrigger" + className; // Например, "OnTriggerFirstDevQuestStep1"

            if (EventManager.Instance?.QuestEvents == null)
            {
                Debug.LogError("QuestEvents не доступен!");
                return;
            }

            EventInfo eventInfo = typeof(QuestEvents).GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);
            if (eventInfo == null)
            {
                Debug.LogError($"Событие {eventName} не найдено в QuestEvents!");
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
            Debug.Log($"Успешно подписались на {eventName}");
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
                Debug.LogError($"Событие {eventName} не найдено при отписке!");
                return;
            }

            // Отписываемся
            eventInfo.RemoveEventHandler(EventManager.Instance.QuestEvents, _subscribedHandler);
            _subscribedHandler = null; // Очищаем ссылку
            Debug.Log($"Успешно отписались от {eventName}");
        }
        
        //using this method we can handle quest step progress (collect items\click on something\end conversation etc) 
        //we can pass a parameter providing branching within the quest step and fail parameter to fail step
        protected abstract void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed);

        //invokes this method after initialization in Quest class. Can be used to fill custom values for quest step
        protected abstract void SetQuestStepState(QuestStepValues questStepValues); 
    }
}
