using UnityEngine;
using UnityEngine.Events;

public class EventComponent : MonoBehaviour
{
    public UnityEvent OnEvent;

    public void EventInvoking()
    {
        OnEvent.Invoke();
    }
}
