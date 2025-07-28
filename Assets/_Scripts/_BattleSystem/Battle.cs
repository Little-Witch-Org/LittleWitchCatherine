using UnityEngine;

namespace _Scripts._BattleSystem
{
    /// <summary>
    /// container with battle info (noMono + scriptable + parsing in manager ?)
    /// </summary>
    public class Battle : MonoBehaviour
    {
        public string battleId;



        public void OnStartCurrentBattle()
        {
            Debug.Log($"Starting Battle  class invocation {battleId}");
        }

        public void OnEndCurrentBattle()
        {
            Debug.Log($"Finishing Battle class invocation {battleId}");
        }
    }
}