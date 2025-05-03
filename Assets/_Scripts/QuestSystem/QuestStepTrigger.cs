using System;
using System.Linq;
using System.Reflection;
using _Scripts.Events;
using _Scripts.Managers;
using _Scripts.Service.Log;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.QuestSystem
{
    /// <summary>
    /// Triggers quest step progress using event (in case of "separate quest step and trigger" logic). Need script with some logic that activates this trigger. //todo add dialogue trigger by event
    /// Automatically find and invoke quest step trigger event by using quest step name 
    /// </summary>
    public class QuestStepTrigger : MonoBehaviour
    {
        
        [Header("Quest Step parameters")]
        [SerializeField] public QuestStep questStep; //uses this field to find dependent event to trigger. Also trigger spawner use it for instantiation depending on step name.
        [SerializeField] private string customParam = "param not implemented for this step"; //For Quest Step logic to handle
        [SerializeField] private bool isFailedParam = false; //For Quest Step logic to handle


        [Header("Trigger Appear Conditions")] [SerializeField]
        public string locationName;
        public string placeName;
        //time
        //reputation
            
        
        [Header("Trigger parameters")]    
        [SerializeField] public bool disableAfterTrigger = true; //disabling trigger object after activation
        [SerializeField] public bool isTriggered; //handle single trigger for object. Also, spawner checks this field to prevent additional activation 
        //todo add option to multi-trigger ?
        
        
        //Method wrapper (for TriggerQuestStep) to invoke method with params (filled in inspector)
        public void TriggerQuestStepInEditor() 
        {
            if (!isTriggered)
            {
                TriggerQuestStep(customParam, isFailedParam);
                
                isTriggered = true;
                
                if (disableAfterTrigger)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void TriggerQuestStep(string customParameter, bool isFailed)
        {
            if (EventManager.Instance?.QuestEvents == null)
            {
                QuestDebug.Instance.LogError("EventManager или QuestEvents не инициализированы!");
                return;
            }

            string methodName = "Trigger" + questStep.name;
            QuestDebug.Instance.Log($"Пытаюсь вызвать метод: {methodName} с параметрами customParameter={customParameter}, isFailed={isFailed}");

            MethodInfo method = typeof(QuestEvents).GetMethod(
                methodName, 
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(string), typeof(bool) }, // Ожидаем метод с двумя параметрами
                null
            );

            if (method != null)
            {
                method.Invoke(EventManager.Instance.QuestEvents, new object[] { customParameter, isFailed });
            }
            else
            {
                QuestDebug.Instance.LogError($"Метод {methodName}(string, bool) не найден.");
            }
        }
    }
}