using UnityEngine;
using UnityEngine.Events;

public class InitializationTriggerActivatorComponent : MonoBehaviour
{
    public UnityEvent OnStart;
    void Start()
    {
        OnStart.Invoke();
    }
}
