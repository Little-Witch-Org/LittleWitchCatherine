using System;
using _Scripts.test.QuestSystem;
using UnityEngine;

public class CollectCoinsQuest_test_QuestStep : QuestStep
{

    //quest step conditions
    private int coinsCollected = 0;
    
    private int coinsToComplete = 2;

    //take info from condition change
    private void OnEnable()
    {
        GameEventsManager_Test.Instance.MiscEventsTest.OnCoinCollected += CoinCollected;
    }
    private void OnDisable()
    {
        GameEventsManager_Test.Instance.MiscEventsTest.OnCoinCollected -= CoinCollected;
    }

    //quest step condition check. If reach it -> finish step
    private void CoinCollected()
    {
        if (coinsCollected < coinsToComplete)
        {
            coinsCollected++;
            UpdateState();
        }

        if (coinsCollected >= coinsToComplete)
        {
            FinishQuesStep(); //from abstract
        }
    }

    //for the saving needs
    private void UpdateState()
    {
        string state = coinsCollected.ToString();
        ChangeState(state);//from abstract
    }


    protected override void SetQuestStepState(string state)
    {
        this.coinsCollected = System.Int32.Parse(state);
        UpdateState();
    }
}
