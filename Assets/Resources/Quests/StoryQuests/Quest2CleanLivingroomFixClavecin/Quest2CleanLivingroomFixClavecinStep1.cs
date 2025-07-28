using _Scripts.LocationsAndPlaces.Places.CatherineHouse;
using _Scripts.Managers;
using _Scripts.QuestSystem;
using UnityEngine;

namespace Resources.Quests.StoryQuests.Quest2CleanLivingroomFixClavecin
{
    /// <summary>
    /// Talk to LivingRoom door step.
    /// </summary>
    public class Quest2CleanLivingroomFixClavecinStep1:QuestStep
    {
        protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
        {
            //not triggers for this step
        }

        protected override void InitializeQuestStepData(QuestStepData questStepData)
        {
            ChangeStepData("", "Нужно сходить в гостиную", false);
            
            //enable living room door npc
            EventManager.Instance.NpcEvents.SetNpcIgnoredStatus("DoorToLivingRoom", false);
            
            //lock exits from house
            EventManager.Instance.LocationsAndPlacesEvents.SetLocationLockState("CatherineHouse",true);
        }

        protected override void InvokesOnFinishQuestStep(bool isFailed)
        {
            ChangeStepData("", "<s>Нужно сходить в гостиную</s>", isFailed);
            EventManager.Instance.NpcEvents.SetNpcIgnoredStatus("DoorToLivingRoom", true);
            EventManager.Instance.NpcEvents.MoveNpc("DoorToLivingRoom", "CatherineHouse", "LivingRoom");
            EventManager.Instance.DialogueEvents.SetDialogueAutoActivation("DoorToLivingRoom",true);
            EventManager.Instance.LocationsAndPlacesEvents.SetPlaceState("LivingRoom",LivingRoom.PlaceStateEnum.MessUnbrokenClavi);
            
        }
    }
}