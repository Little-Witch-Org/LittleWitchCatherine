using _Scripts.QuestSystem;
using UnityEngine;

namespace Resources.Quests.DevQuests.SecondDevQuest
{
    public class SecondDevQuestStep2 : QuestStep
    {
        
        protected override void Start()
        {
            base.Start();

            //handle auto fail option 
            if (IsPreviousFailed && failIfPreviousFailed)
            {
                ChangeValues("Чердак не был посещён", "Я провалила задание, пора возвращаться.", true); //this is not displayed in log cause it hides by get full info method in quest
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
                ChangeValues("Чердак не был посещён","Я спустилась в подвал вопреки запрета 2",true );
                FinishQuesStep();
            }
            
            if (customParam == "AtticRoomVisited")
            {
                ChangeValues("Чердак посещён","Я слазила на чердак, пора возвращаться!",false );
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