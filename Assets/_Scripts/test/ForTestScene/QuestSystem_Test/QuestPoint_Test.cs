using UnityEngine;

namespace _Scripts.test.QuestSystem
{
    //todo change to outside triggers (like click or enter trigger in collidableEvents)
    [RequireComponent(typeof(CircleCollider2D))]
    public class QuestPoint_Test : MonoBehaviour
    {
        
        [Header("Quest_Test")]
        [SerializeField] private QuestInfoSo_Test questInfoForPoint;

        [Header("Quest_Test")] [SerializeField] private bool startPoint = true;
        [Header("Quest_Test")] [SerializeField] private bool finishPoint = true;
        
        
        private bool playerIsNear = false;
        private string questId;
        
        private QuestStateEnum_Test _currentQuestStateEnumTest;
        
        private QuestIcon_Test _questIconTest;

        private void Awake()
        {
            questId = questInfoForPoint.Id;
            _questIconTest = GetComponentInChildren<QuestIcon_Test>();
        }

        private void OnEnable()
        {
            GameEventsManager_Test.Instance.QuestEventsTest.OnQuestStateChange += QuestStateChange;
            GameEventsManager_Test.Instance.InputEventsTest.OnSubmitPressed += SubmitPressed;
        }
        
        private void OnDisable()
        {
            GameEventsManager_Test.Instance.QuestEventsTest.OnQuestStateChange -= QuestStateChange;
            GameEventsManager_Test.Instance.InputEventsTest.OnSubmitPressed -= SubmitPressed;

        }

        //todo need to handle dialogue end / click on object / current time with reached conditions etc
        private void SubmitPressed()
        {
            if (!playerIsNear)
            {
                return;
            }

            //start or finish a quest
            if (_currentQuestStateEnumTest.Equals(QuestStateEnum_Test.CanStart) && startPoint)
            {
                GameEventsManager_Test.Instance.QuestEventsTest.StartQuest(questId);
            }
            else if (_currentQuestStateEnumTest.Equals(QuestStateEnum_Test.CanFinish) && finishPoint)
            {
                GameEventsManager_Test.Instance.QuestEventsTest.FinishQuest(questId);
            }
        }

        private void QuestStateChange(Quest_Test questTest)
        {
            //only update the questTest stateEnumTest if this point has the corresponding questTest
            if (questTest.info.Id.Equals(questId))
            {
                _currentQuestStateEnumTest = questTest.StateEnumTest;
                //Debug.Log("Quest_Test with id: "+ questId+ " stateEnumTest: "+ _currentQuestStateEnumTest);
                _questIconTest.SetState(_currentQuestStateEnumTest, startPoint, finishPoint);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsNear = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsNear = false;
            }
        }
    }
}