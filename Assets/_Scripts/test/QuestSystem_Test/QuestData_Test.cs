using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.test.QuestSystem
{
    [Serializable]
    public class QuestData_Test
    {
        [FormerlySerializedAs("state")] public QuestState_Test stateTest;
        public int questStepIndex;
        public QuestStepState_Test[] questStepStates;

        public QuestData_Test(QuestState_Test stateTest, int questStepIndex, QuestStepState_Test[] questStepStates)
        {
            this.stateTest = stateTest;
            this.questStepIndex = questStepIndex;
            this.questStepStates = questStepStates;
        }
    }
}