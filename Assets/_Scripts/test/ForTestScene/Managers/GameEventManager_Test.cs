using _Scripts.Dialog_Ink;
using _Scripts.events_test;
using _Scripts.test.QuestSystem;
using UnityEngine;

/// <summary>
/// Used as global events bus (for test). Configured in project settings -> script execution order before Default Time
/// to other scripts can subscribe in OnEnable
/// </summary>
public class GameEventsManager_Test : MonoBehaviour
{
    public static GameEventsManager_Test Instance { get; private set; }
    
    public GoldEvents_Test GoldEventsTest;
    public MiscEvents_Test MiscEventsTest;
    public ExpEvents_Test ExpEventsTest;
    public InputEvents_Test InputEventsTest;
    
    public QuestEvents_Test QuestEventsTest;
    

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        // initialize all events
        GoldEventsTest = new GoldEvents_Test();
        MiscEventsTest = new MiscEvents_Test();
        ExpEventsTest = new ExpEvents_Test();
        InputEventsTest = new InputEvents_Test();
        
        QuestEventsTest = new QuestEvents_Test();
    }
}