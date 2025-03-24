using System;
using UnityEngine;


namespace _Scripts.test.QuestSystem
{
    [Serializable]
    public class QuestStepState_Test
    {
        public string state;

        public QuestStepState_Test(string state)
        {
            this.state = state;
        }

        public QuestStepState_Test()
        {
            this.state = "";
        }
    }
}