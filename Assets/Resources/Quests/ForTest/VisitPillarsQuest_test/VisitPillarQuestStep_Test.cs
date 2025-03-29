using System;
using _Scripts.QuestSystem;
using _Scripts.test.QuestSystem;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class VisitPillarQuestStep_Test : QuestStep_Test
{

    [Header("Config")] 
    [SerializeField] private string pillarNumberString = "first";


    private void Start()
    {
        string status = "Visit the " + pillarNumberString + " pillar";
        ChangeState("",status);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string status = "The " + pillarNumberString + " pillar has been visited";
            ChangeState("",status);
            FinishQuesStep();
        }
    }


    protected override void SetQuestStepState(string state)
    {
        //no quest state needed for this quest step 
    }
}
