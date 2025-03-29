using _Scripts.Dialog_Ink;
using _Scripts.Events;
using _Scripts.QuestSystem;
using UnityEngine;

/// <summary>
/// Used as global events bus (for test). Configured in project settings -> script execution order before Default Time
/// to other scripts can subscribe in OnEnable
/// </summary>
public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public InputEvents InputEvents;
    public TransitionEvents TransitionEvents;
    public TimeEvents TimeEvents;
    public QuestEvents QuestEvents;

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
        InputEvents = new InputEvents();
        TransitionEvents = new TransitionEvents();
        TimeEvents = new TimeEvents();
        QuestEvents = new QuestEvents();
    }
}