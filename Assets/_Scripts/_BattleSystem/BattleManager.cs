using System;
using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts._BattleSystem
{ /// <summary>
 /// todo ui must contain buttons characters etc. need to try add positions on some place sprite and character models and check how it works.
 /// todo battle class with information. Do we need scriptable?
 /// todo invocation method after battle finished (rewards, events.other methods etc)
 /// todo add spawn infoSo or class for spawn positions?
 /// </summary>
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;
        
        [SerializeField] private List<Battle> battles;

        [Header("Dynamic variables")] [SerializeField]
        private Battle currentBattle;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            EventManager.Instance.BattleEvents.OnStartBattle += StartBattle;
            EventManager.Instance.BattleEvents.OnFinishBattle += FinishBattle;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.BattleEvents.OnStartBattle += StartBattle;
            EventManager.Instance.BattleEvents.OnFinishBattle -= FinishBattle;
        }

        private void StartBattle(string battleId)
        {
            var battle =  battles.Find((x)=> x.battleId == battleId );
            if (battle != null)
            {
                currentBattle = battle;
            }
            else
            {
                Debug.LogError($"There is no battle with id {battleId}");
            }
            currentBattle.OnStartCurrentBattle();
            EventManager.Instance.BattleEvents.BattleStarted(currentBattle);
        }
        
        private void FinishBattle(string battleId)
        {
            EventManager.Instance.BattleEvents.BattleFinished(currentBattle);
            currentBattle.OnEndCurrentBattle();
        }
    }
}