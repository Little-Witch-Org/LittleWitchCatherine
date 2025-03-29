


    using _Scripts.QuestSystem;
    using UnityEngine;

    namespace Resources.Quests.DevQuests.GoClickOnTriggerAndReturn
    {
        public class FirstDevQuestStep1 : QuestStep
        {
 
            protected override void Start()
            {
                failIfPreviousFailed = false;
                base.Start();
                
                ChangeValues("", "Сходить в оранжерею и нажать на кружок", false);
            }

            private void UpdateStep()
            {
                ChangeValues("", "Кружок в оранжереи нажат",false);
                FinishQuesStep();
            }


            protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
            {
                UpdateStep();
            }

            protected override void SetQuestStepState(QuestStepValues questStepValues)
            {
                //no implementation (nothing to load)
            }
        }
    }
