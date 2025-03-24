using _Scripts.Dialog_Ink;
using _Scripts.events_test;
using _Scripts.Events;
using _Scripts.test.QuestSystem;
using UnityEngine;

/// <summary>
/// Used as global events bus (for test). Configured in project settings -> script execution order before Default Time
/// to other scripts can subscribe in OnEnable
/// </summary>
public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public InputEvents inputEvents;
    public TransitionEvents transitionEvents;
    public TimeEvents timeEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            InitializeEvents();
            DontDestroyOnLoad(gameObject);
        }
        
 
    }
    
    // initialize all events
    private void InitializeEvents()
    {
        //Debug.Log("event manager id " + gameObject.GetInstanceID());
        inputEvents = new InputEvents();
        transitionEvents = new TransitionEvents();
        timeEvents = new TimeEvents();
    }
}