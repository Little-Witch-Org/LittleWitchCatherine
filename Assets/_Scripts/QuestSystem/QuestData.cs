using System;
using UnityEngine.Serialization;

namespace _Scripts.QuestSystem
{
    [Serializable]
    public class QuestData
    {
        public QuestStateEnum stateEnum;
        public int questStepIndex;
        public QuestStepValues[] questStepValues;

        public QuestData(QuestStateEnum stateEnum, int questStepIndex, QuestStepValues[] questStepValues)
        {
            this.stateEnum = stateEnum;
            this.questStepIndex = questStepIndex;
            this.questStepValues = questStepValues;
        }
    }
}