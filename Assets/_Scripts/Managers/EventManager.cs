using _Scripts.Events;
using UnityEngine;

namespace _Scripts.Managers
{
    /// <summary>
    /// Used as global events bus. Configured in project settings -> script execution order before Default Time to other scripts can subscribe in OnEnable
    ///todo to add event style documentation and bring all events to united format 
    /// </summary>
    //------------------
    // _isActiveVariable
    // OnVariableActiveChanged
    // SetVariableActive
    //------------------
    //etc..
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        public InputEvents InputEvents;
        public TransitionEvents TransitionEvents;
        public TimeEvents TimeEvents;
        public QuestEvents QuestEvents;
        public DialogueEvents DialogueEvents;
        public CutsceneEvents CutsceneEvents;
        public GameEvents GameEvents;
        public LocationsAndPlacesEvents LocationsAndPlacesEvents;
        public MiscEvents MiscEvents;
        public PlayerStatsEvents PlayerStatsEvents;
        public ReputationEvents ReputationEvents;
        public InventoryEvents InventoryEvents;
    
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
            DialogueEvents = new DialogueEvents();
            CutsceneEvents = new CutsceneEvents();
            GameEvents = new GameEvents();
            LocationsAndPlacesEvents = new LocationsAndPlacesEvents();
            MiscEvents = new MiscEvents();
            PlayerStatsEvents = new PlayerStatsEvents();
            ReputationEvents = new ReputationEvents();
            InventoryEvents = new InventoryEvents();
        }
    }
}