using System;
using UnityEngine;


namespace _Scripts.test.QuestSystem
{
    [Serializable]
    public class QuestStepState_Test
    {
        public string state;
        public string status;

        public QuestStepState_Test(string state,string status)
        {
            this.state = state;
            this.status = status;
        }

        public QuestStepState_Test()
        {
            this.state = "";
            this.status = "";
        }
    }
}