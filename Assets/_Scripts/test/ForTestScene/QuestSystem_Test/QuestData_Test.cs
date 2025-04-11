using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.test.QuestSystem
{
    [Serializable]
    public class QuestData_Test
    {
        [FormerlySerializedAs("stateTest")] [FormerlySerializedAs("stateEnum")] public QuestStateEnum_Test stateEnumTest;
        public int questStepIndex;
        public QuestStepState_Test[] questStepStates;

        public QuestData_Test(QuestStateEnum_Test stateEnumTest, int questStepIndex, QuestStepState_Test[] questStepStates)
        {
            this.stateEnumTest = stateEnumTest;
            this.questStepIndex = questStepIndex;
            this.questStepStates = questStepStates;
        }
    }
}