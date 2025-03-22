using System;
using UnityEngine;

namespace _Scripts
{
    public class Coin_Test: MonoBehaviour
    {
        public int goldAmount = 1;
        
        /*public static Action<int> OnCoinCollected;

        //handled when player pick up it
        public void CollectCoin()
        {
            Debug.Log("Coin_Test collected");
            OnCoinCollected?.Invoke(goldAmount);
            Destroy(gameObject);
        }*/
        
        public void CollectCoin()
        {
            //Debug.Log("Coin_Test collected");
            GameEventsManager_Test.Instance.MiscEventsTest.CoinCollected();
            GameEventsManager_Test.Instance.GoldEventsTest.GoldGained(goldAmount);
            Destroy(gameObject);
        }
    }
}