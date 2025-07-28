using System;

namespace _Scripts.QuestSystem
{
    /// <summary>
    /// Uses to safe step data in quest StepData[].
    /// Can be used by next steps to check previous step data (isFailed etc..)
    /// can be serialized?
    /// </summary>
    [Serializable]
    public class QuestStepData
    {
        public string stepProgress;  //can be used for counting some things for step (1 of 3 items to collect etc) or sub objectives
        public string stepObjective; //can be used to describe what needs to be done in this step or what has already been done
        public bool isFailed; 

        public QuestStepData(string stepProgress,string stepObjective,bool isFailed)
        {
            this.stepProgress = stepProgress;
            this.stepObjective = stepObjective;
            this.isFailed = isFailed;
        }

        public QuestStepData()
        {
            this.stepProgress = "";
            this.stepObjective = "";
            this.isFailed = false;
        }
    }
}