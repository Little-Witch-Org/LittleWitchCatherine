using _Scripts.QuestSystem;
using UnityEngine;

namespace Resources.Quests.DevQuests.SecondDevQuest
{
    public class SecondDevQuestStep2 : QuestStep
    {
        
        protected override void Start()
        {
            base.Start();

            //handle auto fail option  //todo need to handle auto failed steps in quest log. Need to hide if we fail depending step and auto fail next steps.
            if (IsPreviousFailed && failIfPreviousFailed)
            {
                ChangeValues("Чердак не был посещён", "Я провалила задание, пора возвращаться.", true);
                FinishQuesStep();
            }
            else
            {
                ChangeValues("Чердак не посещён", "Мне нужно сходить на чердак.", false);
            }

        }
        
        protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
        {
            if (isFailed)
            {
                ChangeValues("Чердак не был посещён","Я провалила задание, пора возвращаться.",true );
                FinishQuesStep();
            }
            
            if (customParam == "AtticRoomVisited")
            {
                ChangeValues("Чердак посещён","Задание выполнено, пора возвращаться!",false );
                FinishQuesStep();
            }
            
            
        }

        //set IsPreviousFailed value depending on previous step value
        protected override void SetQuestStepState(QuestStepValues questStepValues)
        {
            if (questStepValues.isFailed)
            {
                IsPreviousFailed = true;
            }
        }
    }
}