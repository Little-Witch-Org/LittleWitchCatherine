using _Scripts.Managers;
using _Scripts.QuestSystem;
using UnityEngine;

namespace _Scripts.Components._TriggerActivators.ForQuest
{
    public class PlayerVisitsNovelPlaceTriggerActivator : MonoBehaviour
    {
        [SerializeField] private QuestStepTrigger triggerToActivate;
        [SerializeField] private string placeToVisit;


        private void Start()
        {
            //Debug.Log(PlayerCharacterManager.Instance.GetCurrentNovelPlace());
            if (PlayerCharacterManager.Instance.GetCurrentNovelPlace() == placeToVisit)
            {
                //Debug.Log("Character Visits Novel Place Trigger");
                triggerToActivate.TriggerQuestStepInEditor();
            }
        }
    }
}