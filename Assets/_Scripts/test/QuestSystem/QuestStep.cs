using UnityEngine;

namespace _Scripts.test.QuestSystem
{
    public abstract class QuestStep : MonoBehaviour
    {

    
        private bool _isFinished = false;

        private string _questId;

        private int _stepIndex;

        public void InitializeQuestStep(string questId, int stepIndex, string questStepState)
        {
            _questId = questId;
            _stepIndex = stepIndex;

            if (questStepState != null && questStepState != "")
            {
                SetQuestStepState(questStepState);
            }
        }

        protected void FinishQuesStep()
        {
            if (!_isFinished)
            {
                _isFinished = true;
            
                GameEventsManager_Test.Instance.QuestEvents.AdvanceQuest(_questId);
            
                Destroy(gameObject);
            }
        }

        protected void ChangeState(string newState)
        {
            GameEventsManager_Test.Instance.QuestEvents.QuestStepStateChange(_questId, _stepIndex, new QuestStepState(newState));
        }
        
        protected abstract void SetQuestStepState(string state);
    }
}
