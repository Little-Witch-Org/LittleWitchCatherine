using _Scripts.LocationsAndPlaces.Places.CatherineHouse;
using _Scripts.Managers;
using _Scripts.QuestSystem;

namespace Resources.Quests.StoryQuests.Quest2CleanLivingroomFixClavecin
{/// <summary>
 /// Entering LivingRoom step
 /// </summary>
    public class Quest2CleanLivingroomFixClavecinStep2:QuestStep
    {
        protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
        {
            //no triggers
        }

        protected override void InitializeQuestStepData(QuestStepData questStepData)
        {
            ChangeStepData("", "Нужно прибрать в комнате", false);
        }

        protected override void InvokesOnFinishQuestStep(bool isFailed)
        {
            ChangeStepData("", "<s>Нужно прибрать в комнате</s>", isFailed);
            EventManager.Instance.LocationsAndPlacesEvents.SetPlaceState("GuestBedroom", GuestBedroom.PlaceStateEnum.Default);
        }
    }
}