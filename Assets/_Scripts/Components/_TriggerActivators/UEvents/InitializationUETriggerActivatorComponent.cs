using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Components._TriggerActivators.UEvents
{
    public class InitializationUeTriggerActivatorComponent : MonoBehaviour
    {
        public UnityEvent OnStart;
        void Start()
        {
            OnStart.Invoke();
        }
    }
}
