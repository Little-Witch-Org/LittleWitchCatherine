using System;
using _Scripts.test.QuestSystem;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class VisitPillarsQuest_test_QuestStep : QuestStep
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
        //no quest state needed for this quest step
    }
}
