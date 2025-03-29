using System;
using _Scripts.QuestSystem;
using _Scripts.test.QuestSystem;
using UnityEngine;

public class CollectCoinsQuestStep_Test : QuestStep_Test
{

    //quest step conditions
    private int coinsCollected = 0;
    
    private int coinsToComplete = 2;

    private void Start()
    {
        UpdateState();
    }


    //take InfoSo from condition change
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
        string status = "CoinsCollected: " + coinsCollected + " / " + coinsToComplete + " coins";
        ChangeState(state, status);//from abstract
    }


    protected override void SetQuestStepState(string state)
    {
        this.coinsCollected = System.Int32.Parse(state);
        UpdateState();
    }
}
