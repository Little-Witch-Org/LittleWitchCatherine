using _Scripts.Managers;
using _Scripts.QuestSystem;
using UnityEngine;

namespace Resources.Quests.StoryQuests.Quest2CleanLivingroomFixClavecin
{
    /// <summary>
    /// Fight with clavi step (finishes after battle using results (win/loose))
    /// </summary>
    public class Quest2CleanLivingroomFixClavecinStep3:QuestStep
    {
        protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
        {
            //no triggers for step
        }

        protected override void InitializeQuestStepData(QuestStepData questStepData)
        {
            ChangeStepData("", "Нужно разобраться с духом", false);
        }

        protected override void InvokesOnFinishQuestStep(bool isFailed)
        {
            if (isFailed)
            {
                ChangeStepData("", "Нужно разобраться с духом", true);
                //todo to add bed regime quest
                EventManager.Instance.CutsceneEvents.LaunchCutscene("WakeUpAfterClaviLooseCutscene");
            }
            else
            {
                ChangeStepData("", "<s>Нужно разобраться с духом</s>", false);
            }
            
        }
    }
}