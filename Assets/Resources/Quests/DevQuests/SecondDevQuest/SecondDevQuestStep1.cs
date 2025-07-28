using _Scripts.QuestSystem;
using UnityEngine;

namespace Resources.Quests.DevQuests.SecondDevQuest
{
    public class SecondDevQuestStep1 : QuestStep
    {
    
        [SerializeField]private bool isParentRoomVisited = false;
        [SerializeField]private bool isBrotherRoomVisited = false;
    
    
        protected override void Start()
        {
            failIfPreviousFailed = false;
            base.Start();
                
            ChangeStepData("Посещены комнаты = 0/2", "Мне нужно сходить в комнату родителей и брата", false);
        }
    
    
        protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
        {
            if (customParam == "ParentRoomVisited")
            {
                isParentRoomVisited = true;
                ChangeStepData("Посещены комнаты = 1/2","Нужно сходить в комнату брата",false );
            }
            if (customParam == "BrotherRoomVisited")
            {
                isBrotherRoomVisited = true;
                ChangeStepData("Посещены комнаты = 1/2","Нужно сходить в комнату родителей",false );
            }
            if (isFailed)
            {
                ChangeStepData("какие комнаты посещены уже не важно","Я спустилась в подвал вопреки запрета 1",true );
                FinishQuesStep(true);
            }

            if (isParentRoomVisited && isBrotherRoomVisited)
            {
                ChangeStepData("Посещены комнаты = 2/2","Обе комнаты посещены",false );
                FinishQuesStep(false);
            }
        
        }

        protected override void InitializeQuestStepData(QuestStepData questStepData)
        {
            //no pre reqs
        }

        protected override void InvokesOnFinishQuestStep(bool isFailed)
        {
            throw new System.NotImplementedException();
        }
    }
}
