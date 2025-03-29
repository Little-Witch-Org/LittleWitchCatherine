using System;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.QuestSystem
{
    /// <summary>
    /// Triggers quest step progress using event (in case of "separate quest step and trigger" logic). Need script with some logic that activates this trigger.
    /// Automatically find and invoke quest step trigger event by using quest step name 
    /// </summary>
    public class QuestStepTrigger : MonoBehaviour
    {
        [SerializeField] private QuestStep questStep; //uses this field to find dependent event to trigger
        [SerializeField] private bool isTriggered; //handle single trigger for object (in current place prefab before transition) //todo need to handle by using spawner
        
        [Header("Parameters")]
        [SerializeField] private string customParamInInspector = "param not implemented for this step";
        [SerializeField] private bool isFailedInInspector = false;
        
        public void TriggerQuestStepInEditor()
        {
            if (!isTriggered)
            {
                Debug.Log(questStep.name+ " trigger invoked with parameters "+customParamInInspector+"/"+ isFailedInInspector);
                TriggerQuestStep(customParamInInspector, isFailedInInspector);
                isTriggered = true;
            }
        }

        private void TriggerQuestStep(string customParam, bool isFailed)
        {
            if (EventManager.Instance?.QuestEvents == null)
            {
                Debug.LogError("EventManager или QuestEvents не инициализированы!");
                return;
            }

            string methodName = "Trigger" + questStep.name;
            Debug.Log($"Пытаюсь вызвать метод: {methodName} с параметрами customParam={customParam}, isFailed={isFailed}");

            MethodInfo method = typeof(QuestEvents).GetMethod(
                methodName, 
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(string), typeof(bool) }, // Ожидаем метод с двумя параметрами
                null
            );

            if (method != null)
            {
                method.Invoke(EventManager.Instance.QuestEvents, new object[] { customParam, isFailed });
            }
            else
            {
                Debug.LogError($"Метод {methodName}(string, bool) не найден.");
            }
        }
    }
}