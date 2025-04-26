using System;
using UnityEngine;

//todo try context for this case
namespace _Scripts.QuestSystem
{
    /// <summary>
    /// test trigger (red square for dialogue npc 1 / quest 2 ) 
    /// </summary>
    public class QuestCanStartEnablerTrigger : MonoBehaviour
    {
        [SerializeField] private QuestInfoSo questInfo;
        private bool _isInDialogue;

        private void OnEnable()
        {
            var dialogueEvents = EventManager.Instance.DialogueEvents;
            dialogueEvents.OnDialogueStarted += OnDialogueStarted;
            dialogueEvents.OnDialogueFinished += OnDialogueFinished;
        }

        private void OnDisable()
        {
            var dialogueEvents = EventManager.Instance.DialogueEvents;
            dialogueEvents.OnDialogueStarted -= OnDialogueStarted;
            dialogueEvents.OnDialogueFinished -= OnDialogueFinished;
        }

        public void ChangeQuestAvailability(bool isAvailable)
        {
            if (CanChangeQuestAvailability())
            {
                EventManager.Instance.QuestEvents.QuestAvailabilityChange(questInfo.Id, isAvailable);
            }
        }

        private bool CanChangeQuestAvailability() => !_isInDialogue;

        private void OnDialogueStarted() => _isInDialogue = true;
        private void OnDialogueFinished() => _isInDialogue = false;
    }
}