using _Scripts.Managers;
using _Scripts.QuestSystem;
using UnityEngine;

namespace Resources.Quests.StoryQuests.Quest3GetGreeneryFromGarden
{
    public class Quest3GetGreeneryFromGardenStep1:QuestStep
    {
        protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
        {
            //todo to add
        }

        protected override void InitializeQuestStepData(QuestStepData questStepData)
        {
            //todo to add
        }

        protected override void InvokesOnFinishQuestStep(bool isFailed)
        {
            if (isFailed)
            {
                ChangeStepData("","",true);
            }
        }

        protected override void InvokesAfterAdvanceQuest()
        {
            EventManager.Instance.QuestEvents.FinishQuest("Quest3GetGreeneryFromGarden");
        }
    }
}