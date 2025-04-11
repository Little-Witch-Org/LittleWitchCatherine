using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Transition animation triggers it in last frame
/// </summary>
public class AnimationTriggerActivatorForSceneExit : MonoBehaviour
{
    public UnityEvent OnEvent;

    public void EventInvoking()
    {
        OnEvent.Invoke();
    }
}
