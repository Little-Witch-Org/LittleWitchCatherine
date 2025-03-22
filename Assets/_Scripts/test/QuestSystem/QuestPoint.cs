using UnityEngine;

namespace _Scripts.test.QuestSystem
{
    //todo change to outside triggers (like click or enter trigger in collidableEvents)
    [RequireComponent(typeof(CircleCollider2D))]
    public class QuestPoint : MonoBehaviour
    {
        
        [Header("Quest")]
        [SerializeField] private QuestInfoSo questInfoForPoint;

        [Header("Quest")] [SerializeField] private bool startPoint = true;
        [Header("Quest")] [SerializeField] private bool finishPoint = true;
        
        
        private bool playerIsNear = false;
        private string questId;
        
        private QuestState currentQuestState;
        
        private QuestIcon questIcon;

        private void Awake()
        {
            questId = questInfoForPoint.Id;
            questIcon = GetComponentInChildren<QuestIcon>();
        }

        private void OnEnable()
        {
            GameEventsManager_Test.Instance.QuestEvents.OnQuestStateChange += QuestStateChange;
            GameEventsManager_Test.Instance.InputEventsTest.OnSubmitPressed += SubmitPressed;
        }
        
        private void OnDisable()
        {
            GameEventsManager_Test.Instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
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
            if (currentQuestState.Equals(QuestState.CanStart) && startPoint)
            {
                GameEventsManager_Test.Instance.QuestEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CanFinish) && finishPoint)
            {
                GameEventsManager_Test.Instance.QuestEvents.FinishQuest(questId);
            }
        }

        private void QuestStateChange(Quest quest)
        {
            //only update the quest state if this point has the corresponding quest
            if (quest.info.Id.Equals(questId))
            {
                currentQuestState = quest.State;
                //Debug.Log("Quest with id: "+ questId+ " state: "+ currentQuestState);
                questIcon.SetState(currentQuestState, startPoint, finishPoint);
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