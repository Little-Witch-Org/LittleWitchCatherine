using System;
using _Scripts.test.QuestSystem;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class VisitPillarQuest_test_QuestStep : QuestStep_Test
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FinishQuesStep();
        }
    }


    protected override void SetQuestStepState(string state)
    {
        //no quest stateTest needed for this quest step
    }
}
