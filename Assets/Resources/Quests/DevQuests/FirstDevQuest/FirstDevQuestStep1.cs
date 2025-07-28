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
                
                ChangeStepData("Кружок не нажат", "Сходить в оранжерею и нажать на кружок", false);
            }

            private void UpdateStep()
            {
                ChangeStepData("Кружок нажат", "Задача выполнена, пора возвращаться",false);
                FinishQuesStep(false);
            }


            protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
            {
                UpdateStep();
            }

            protected override void InitializeQuestStepData(QuestStepData questStepData)
            {
                //no implementation (nothing to load)
            }

            protected override void InvokesOnFinishQuestStep(bool isFailed)
            {
                throw new System.NotImplementedException();
            }
        }
    }
