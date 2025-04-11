using System;
using UnityEngine;

namespace _Scripts.QuestSystem
{
    /// <summary>
    /// Point (place/Character/Trigger) for starting or finishing quest
    /// TODO since we use dialogue to start/finish quests. This can be used for "map scene and collider trigger of player char model". But we also can trigger dialogue =)
    /// </summary>
    public class QuestPoint : MonoBehaviour
    {
        //[Header("Dialogue (optional)")] [SerializeField]
        //private string dialogueKnotName;

        [Header("Quest")] [SerializeField] private QuestInfoSo questInfoForPoint;

        [Header("Quest")] [SerializeField] private bool startPoint = true;
        [Header("Quest")] [SerializeField] private bool finishPoint = true;


        private bool playerIsNear = false;
        private string questId;

        private QuestStateEnum _currentQuestStateEnum;

        private QuestIcon _questIcon;

        private void Awake()
        {
            questId = questInfoForPoint.Id;
            _questIcon = GetComponentInChildren<QuestIcon>();
        }

        private void Start()
        {
            //CheckQuestStateOnStart();
            UpdateQuestStateOnStart();
        }

        private void OnEnable()
        {
            EventManager.Instance.QuestEvents.OnQuestStateChange += QuestIconStateChange;
            //EventManager.Instance.InputEvents.OnSubmitPressed += SubmitPressed;
        }

        private void OnDisable()
        {
            EventManager.Instance.QuestEvents.OnQuestStateChange -= QuestIconStateChange;
            //EventManager.Instance.InputEvents.OnSubmitPressed -= SubmitPressed;

        }

        //todo need to handle dialogue end / click on object / current time with reached conditions etc
        //todo add scene check - if on map -> need to submit by pressing button if novel -> mouse click
        private void SubmitPressed()
        {
            if (!playerIsNear)
            {
                return;
            }

            //start or finish a quest
            if (_currentQuestStateEnum.Equals(QuestStateEnum.CanStart) && startPoint)
            {
                EventManager.Instance.QuestEvents.StartQuest(questId);
            }
            else if (_currentQuestStateEnum.Equals(QuestStateEnum.CanFinish) && finishPoint)
            {
                EventManager.Instance.QuestEvents.FinishQuest(questId);
            }
        }

        //manual start/finish quest method (by default we use dialogue trigger)
        public void ClickSubmit()
        {
            /*//todo if this have dialogue component -> invoke method from there
            //if we have a knot name defined, try to start dialogue with it
            if (!dialogueKnotName.Equals(string.Empty))
            {
               // EventManager.Instance.DialogueEvents.EnterDialogue(dialogueKnotName);
            }
            else
            {*/
            //Debug.Log("Submit clicked on quest: " + questId);

            //start or finish a quest
            if (_currentQuestStateEnum.Equals(QuestStateEnum.CanStart) && startPoint)
            {
                EventManager.Instance.QuestEvents.StartQuest(questId);
            }
            else if (_currentQuestStateEnum.Equals(QuestStateEnum.CanFinish) && finishPoint)
            {
                EventManager.Instance.QuestEvents.FinishQuest(questId);
            }
            // }

        }

        private void QuestIconStateChange(Quest quest)
        {
            //only update the quest state if this point has the corresponding quest
            if (quest.InfoSo.Id.Equals(questId))
            {
                _currentQuestStateEnum = quest.StateEnum;
                //Debug.Log("Quest with id: "+ questId+ " stateEnum: "+ _currentQuestStateEnum);
                _questIcon.SetState(_currentQuestStateEnum, startPoint, finishPoint);
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
        
        private void UpdateQuestStateOnStart()
        {
            QuestIconStateChange(EventManager.Instance.QuestEvents.RequestQuestByQuestInfoSo(questInfoForPoint));
        }
    }
}