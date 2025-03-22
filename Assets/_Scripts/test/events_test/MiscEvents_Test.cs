using System;
using UnityEngine;

public class MiscEvents_Test
{
    public event Action OnCoinCollected;
    
    public void CoinCollected() 
    {
        //Debug.Log("misc CoinCollected");
        OnCoinCollected?.Invoke();
    }

    
    public event Action OnGemCollected;
    
    public void GemCollected() 
    {
       OnGemCollected?.Invoke();
    }
    
}