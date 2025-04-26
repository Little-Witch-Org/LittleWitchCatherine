using _Scripts.Managers;
using _Scripts.QuestSystem;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Resources.Quests.StoryQuests.Mother.Quest_1_GoDownToKitchen
{
    public class Quest1GoDownToKitchenStep1: QuestStep
    {
        
        protected override void Start()
        {
            failIfPreviousFailed = false;
            base.Start();
                
            ChangeValues("Зайти в гардеробную и переодеться", "Спуститься на кухню на завтрак", false);
        }
        
        
        protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
        {
            if (customParam == "DressingRoomVisited")
            {
                Debug.Log("DressingRoomVisited");
                
                ChangeValues("Я переоделась к завтраку", "Спуститься на кухню на завтрак", false);
                TimeManager.Instance.AddMinutes(15);
            }
            
            if (customParam == "KitchenVisited")
            {
                Debug.Log("KitchenVisited");
                
                ChangeValues("", "Я спустилась на кухню", false);
                TimeManager.Instance.AddMinutes(5);
                
                Debug.Log("quest 1 step 1 - unlocking all places (and location exits) except way to kitchen and dressing room");
                
                EventManager.Instance.LocationsAndPlacesEvents.SetAllPlacesLockState("CatherineHouse", false);
                EventManager.Instance.LocationsAndPlacesEvents.SetAllLocationsLockState(false);
                
                //disable auto activation dialogue with mother
                EventManager.Instance.DialogueEvents.SetDialogueAutoActivation("Mother", false);
                
                FinishQuesStep();
            }
        }

        protected override void SetQuestStepState(QuestStepValues questStepValues)
        {
            Debug.Log("quest 1 step 1 - locking all places (and location exits) except way to kitchen and dressing room");

            EventManager.Instance.LocationsAndPlacesEvents.SetAllPlacesLockState("CatherineHouse", true);
            
            EventManager.Instance.LocationsAndPlacesEvents.SetPlacesLockState("CatherineHouse", false,
                CatherineHouseNovelViewPlaces.CatherineRoom.ToString(),
                CatherineHouseNovelViewPlaces.CatherineDressingRoom.ToString(),
                CatherineHouseNovelViewPlaces.TFCorridor.ToString(),
                CatherineHouseNovelViewPlaces.SFCorridor.ToString(),
                CatherineHouseNovelViewPlaces.FFCorridor.ToString(),
                CatherineHouseNovelViewPlaces.Kitchen.ToString()
            );
            
            EventManager.Instance.LocationsAndPlacesEvents.SetLocationLockState("CatherineHouse",true);
            
            //enable auto activation dialogue with mother
            EventManager.Instance.DialogueEvents.SetDialogueAutoActivation("Mother", true);
        }
    }
}