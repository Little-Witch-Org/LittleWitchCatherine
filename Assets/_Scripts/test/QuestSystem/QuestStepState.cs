using System;
using UnityEngine;


namespace _Scripts.test.QuestSystem
{
    [Serializable]
    public class QuestStepState
    {
        public string state;

        public QuestStepState(string state)
        {
            this.state = state;
        }

        public QuestStepState()
        {
            this.state = "";
        }
    }
}