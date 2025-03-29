using System;

namespace _Scripts.QuestSystem
{
    /// <summary>
    /// Uses to safe step values in quest StepStates[] (cause it single-shot?).
    /// Can be used by next steps to check previous step status (isFailed etc..)
    /// </summary>
    [Serializable]
    public class QuestStepValues
    {
        public string state;
        public string status;
        public bool isFailed;

        public QuestStepValues(string state,string status,bool isFailed)
        {
            this.state = state;
            this.status = status;
            this.isFailed = isFailed;
        }

        public QuestStepValues()
        {
            this.state = "";
            this.status = "";
            this.isFailed = false;
        }
    }
}