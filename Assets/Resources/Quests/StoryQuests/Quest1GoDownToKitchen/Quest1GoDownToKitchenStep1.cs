using _Scripts.Enums.Places;
using _Scripts.Managers;
using _Scripts.QuestSystem;
using UnityEngine;

namespace Resources.Quests.StoryQuests.Quest1GoDownToKitchen
{
    public class Quest1GoDownToKitchenStep1: QuestStep
    {
        
        protected override void Start()
        {
            failIfPreviousFailed = false;
            base.Start();
                
            ChangeStepData("Зайти в гардеробную и переодеться", "Спуститься на кухню на завтрак", false);
        }
        
        
        protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
        {
            if (customParam == "DressingRoomVisited")
            {
                Debug.Log("DressingRoomVisited");
                
                ChangeStepData("<s>Зайти в гардеробную и переодеться</s>", "Спуститься на кухню на завтрак", false);
                TimeManager.Instance.AddMinutes(15);
            }
            
            if (customParam == "KitchenVisited")
            {
                Debug.Log("KitchenVisited");
                
                ChangeStepData("", "<s>Спуститься на кухню на завтрак</s>", false);
                TimeManager.Instance.AddMinutes(5);
                
                FinishQuesStep(false);
            }
        }

        protected override void InitializeQuestStepData(QuestStepData questStepData)
        {
            Debug.Log("quest 1 step 1 - locking all places (and location exits) except way to kitchen and dressing room");

            EventManager.Instance.LocationsAndPlacesEvents.SetAllPlacesLockState("CatherineHouse", true);
            
            EventManager.Instance.LocationsAndPlacesEvents.SetPlacesLockState("CatherineHouse", false,
                CatherineHouseNovelViewPlacesEnum.CatherineRoom.ToString(),
                CatherineHouseNovelViewPlacesEnum.CatherineDressingRoom.ToString(),
                CatherineHouseNovelViewPlacesEnum.TFCorridor.ToString(),
                CatherineHouseNovelViewPlacesEnum.SFCorridor.ToString(),
                CatherineHouseNovelViewPlacesEnum.FFCorridor.ToString(),
                CatherineHouseNovelViewPlacesEnum.Kitchen.ToString()
            );
            
            EventManager.Instance.LocationsAndPlacesEvents.SetLocationLockState("CatherineHouse",true);
            
            //enable auto activation dialogue with mother
            EventManager.Instance.DialogueEvents.SetDialogueAutoActivation("Mother", true);
            
        }

        protected override void InvokesOnFinishQuestStep(bool isFailed)
        {
            Debug.Log("quest 1 step 1 - unlocking all places (and location exits) except way to kitchen and dressing room");
                
            EventManager.Instance.LocationsAndPlacesEvents.SetAllPlacesLockState("CatherineHouse", false);
            EventManager.Instance.LocationsAndPlacesEvents.SetAllLocationsLockState(false);
                
            //disable auto activation dialogue with mother
            EventManager.Instance.DialogueEvents.SetDialogueAutoActivation("Mother", false);
            
            //start 2nd quest
            EventManager.Instance.QuestEvents.StartQuest("Quest2CleanLivingroomFixClavecin");
                
            //make next quests visible
            EventManager.Instance.QuestEvents.SetQuestVisibility("Quest2CleanLivingroomFixClavecin",true);
            EventManager.Instance.QuestEvents.SetQuestVisibility("Quest3GetGreeneryFromGarden",true);
            EventManager.Instance.QuestEvents.SetQuestVisibility("Quest4ChangeStormCrystalOnBathhouse",true);
            EventManager.Instance.QuestEvents.SetQuestVisibility("Quest5CheckThePit",true);
            EventManager.Instance.QuestEvents.SetQuestVisibility("Quest6CleanTheMapFromGreyMoss",true);
            EventManager.Instance.QuestEvents.SetQuestVisibility("Quest7ComToDinnerAtSevenPM",true);
        }
    }
}