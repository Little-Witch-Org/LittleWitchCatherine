using System;
using _Scripts.QuestSystem;
using UnityEngine;

namespace _Scripts.Components.UEventsTriggers
{
    public class CharacterVisitsNovelPlaceTrigger : MonoBehaviour
    {
        [SerializeField] private QuestStepTrigger triggerToActivate;
        [SerializeField] private string placeToVisit;


        private void Start()
        {
            Debug.Log(PlayerCharacterManager.Instance.GetCurrentNovelPlace());
            if (PlayerCharacterManager.Instance.GetCurrentNovelPlace() == placeToVisit)
            {
                Debug.Log("Character Visits Novel Place Trigger");
                triggerToActivate.TriggerQuestStepInEditor();
            }
        }
    }
}