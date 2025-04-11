using System;
using UnityEngine;

namespace _Scripts
{
    public class ExpGems_Test : MonoBehaviour
    {
        public int exp = 10;

        /*public static Action<int> OnExpCollected;

        //handled when player pick up it
        public void CollectExp()
        {
            Debug.Log("exp gem collected");
            OnExpCollected?.Invoke(exp);
            Destroy(gameObject);
        }
    }*/
        public void CollectExp()
        {
            //Debug.Log("exp gem collected");
            GameEventsManager_Test.Instance.ExpEventsTest.ExperienceGained(exp);
            Destroy(gameObject);
        }
        
    }
}