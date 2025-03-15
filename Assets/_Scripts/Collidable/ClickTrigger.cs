using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


/// <summary>
/// Uses for putting functions on events in Inspector
/// //todo not sure that using  EventSystem.current.IsPointerOverGameObject() is good idea to prevent clicks then UI opened. It also blocks other colliders
/// </summary>
public class ClickTrigger : MonoBehaviour
{
    public UnityEvent OnClick;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private bool _isClicked; //handle double click 

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!_isClicked)
        {
            OnClick.Invoke();
            _isClicked = true;
        }
    }

    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        OnExit.Invoke();
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        OnEnter.Invoke();
    }
}
