using System;
using _Scripts.Enums;
using UnityEngine.Serialization;

namespace _Scripts.QuestSystem
{
    [Serializable]
    public class QuestData
    {
        public QuestStateEnum stateEnum;
        public int questStepIndex;
        public QuestStepData[] questStepValues;

        public QuestData(QuestStateEnum stateEnum, int questStepIndex, QuestStepData[] questStepValues)
        {
            this.stateEnum = stateEnum;
            this.questStepIndex = questStepIndex;
            this.questStepValues = questStepValues;
        }
    }
}