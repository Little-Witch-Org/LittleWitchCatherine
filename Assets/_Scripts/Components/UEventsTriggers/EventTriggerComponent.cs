using UnityEngine;
using UnityEngine.Events;

public class EventTriggerComponent : MonoBehaviour
{
    public UnityEvent OnEvent;

    public void EventInvoking()
    {
        OnEvent.Invoke();
    }
}
