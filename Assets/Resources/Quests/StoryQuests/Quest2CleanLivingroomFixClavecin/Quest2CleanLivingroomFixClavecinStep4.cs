using System;
using System.Collections;
using System.Globalization;
using _Scripts.InventorySystem.ByGuide;
using _Scripts.Managers;
using _Scripts.QuestSystem;
using UnityEngine;

namespace Resources.Quests.StoryQuests.Quest2CleanLivingroomFixClavecin
{
    /// <summary>
    /// //todo доделать диалоги
    /// </summary>
    public class Quest2CleanLivingroomFixClavecinStep4:QuestStep
    {

        private bool _isFailedBattle;
        private void OnEnable()
        {
            EventManager.Instance.PlayerStatsEvents.OnHealthChanged += CheckHealthAndUpdateDialogue;
            EventManager.Instance.InventoryEvents.OnItemUsed += CheckQuestItemUsed;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.PlayerStatsEvents.OnHealthChanged -= CheckHealthAndUpdateDialogue;
            EventManager.Instance.InventoryEvents.OnItemUsed -= CheckQuestItemUsed;
        }

        private void CheckHealthAndUpdateDialogue(float currentHealth)
        {
            float playerHp = PlayerCharacterManager.Instance.CurrentHealth;
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("Mother_StoryMain", "playerHealthOnStartDialogue", playerHp);
        }
        
        private void CheckHealthAndUpdateDialogue()
        {
            float playerHp = PlayerCharacterManager.Instance.CurrentHealth;
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("Mother_StoryMain", "playerHealthOnStartDialogue", playerHp);
            Debug.Log($"-----hp after battle {playerHp} in 2 quest 4 step");
        }

        private void CheckQuestItemUsed(InventoryItem item)
        {
            if (item.itemData.itemId.Equals("inventory_item_id_1_5"))
            {
                ChangeStepData("<s>нужно сходить в кладовку под лестницей, достать йод и подлечиться</s>", "Нужно вернуться к матери и сообщить результат", false);
            }
        }


        protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
        {
            //todo add to heal trigger..
        }

        protected override void InitializeQuestStepData(QuestStepData questStepData)
        {
            //fails on start of step if previous failed
            if (questStepData.isFailed)
            {
                _isFailedBattle = true;
                EventManager.Instance.NpcEvents.DeleteNpc("DoorToLivingRoom");
                StartCoroutine(DelayedFinishQuestStepAsFailed());
            }
            else//quest finishes in mother dialogue 4
            {
                EventManager.Instance.NpcEvents.DeleteNpc("DoorToLivingRoom");
                CheckHealthAndUpdateDialogue();
                ChangeStepData("нужно сходить в кладовку под лестницей, достать йод и подлечиться", "Нужно вернуться к матери и сообщить результат", false);


                //todo add grid to storage under stairs locker and add item to it, add subtask
            }
        }

        private IEnumerator DelayedFinishQuestStepAsFailed() //handle finish in initialization (to consistent advanceQuest invocation after 3 step)
        {
            yield return null;
            FinishQuesStep(true);
        }

        protected override void InvokesOnFinishQuestStep(bool isFailed)
        {
            //unlock house
            EventManager.Instance.LocationsAndPlacesEvents.SetLocationLockState("CatherineHouse",false);
            if (!isFailed)
            {
                ChangeStepData("", "<s>Нужно вернуться к матери и сообщить результат</s>", false);
            }
        }

        protected override void InvokesAfterAdvanceQuest()
        {
            EventManager.Instance.QuestEvents.FinishQuest("Quest2CleanLivingroomFixClavecin");

            if (_isFailedBattle)//fail 3 if battle fails
            {
                EventManager.Instance.QuestEvents.StartQuest(
                    "Quest3GetGreeneryFromGarden"); //start 3 quest (cause it depends on 2 quest which needs to be ended). Note - prints RNM warning cause update in QManager doesn't have time (frames) to update quest state.

                EventManager.Instance.QuestEvents.FinishCurrentQuestStep("Quest3GetGreeneryFromGarden",
                    true); //fail 3 quest step 1
                
                EventManager.Instance.DialogueEvents.CompleteDialogueKnot("Mother","Mother_Dialogue4");
            }
        }
    }
}