using _Scripts.QuestSystem;
using UnityEngine;

public class SecondDevQuestStep1 : QuestStep
{
    
    [SerializeField]private bool isParentRoomVisited = false;
    [SerializeField]private bool isBrotherRoomVisited = false;
    
    
    protected override void Start()
    {
        failIfPreviousFailed = false;
        base.Start();
                
        ChangeValues("Посещены комнаты = 0/2", "Мне нужно сходить в комнату родителей и брата", false);
    }
    
    
    protected override void ActivateSubscribedMethodOnTrigger(string customParam, bool isFailed)
    {
        if (customParam == "ParentRoomVisited")
        {
            isParentRoomVisited = true;
            ChangeValues("Посещены комнаты = 1/2","Нужно сходить в комнату брата",false );
        }
        if (customParam == "BrotherRoomVisited")
        {
            isBrotherRoomVisited = true;
            ChangeValues("Посещены комнаты = 1/2","Нужно сходить в комнату родителей",false );
        }
        if (isFailed)
        {
            ChangeValues("какие комнаты посещены уже не важно","Я спустилась в подвал вопреки запрета 1",true );
            FinishQuesStep();
        }

        if (isParentRoomVisited && isBrotherRoomVisited)
        {
            ChangeValues("Посещены комнаты = 2/2","Обе комнаты посещены",false );
            FinishQuesStep();
        }
        
    }

    protected override void SetQuestStepState(QuestStepValues questStepValues)
    {
        //no pre reqs
    }
}
