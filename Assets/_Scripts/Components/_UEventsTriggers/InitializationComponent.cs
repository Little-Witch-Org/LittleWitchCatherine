using UnityEngine;
using UnityEngine.Events;

public class InitializationComponent : MonoBehaviour
{
    public UnityEvent OnStart;
    void Start()
    {
        OnStart.Invoke();
    }
}
