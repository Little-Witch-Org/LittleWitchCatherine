using System;
using _Scripts._BattleSystem;

namespace _Scripts.Events
{
    public class BattleEvents
    {
        public event Action<string> OnStartBattle;
        public void StartBattle(string battleId)
        {
            OnStartBattle?.Invoke(battleId);
        }
        
        public event Action<Battle> OnBattleStarted;
        public void BattleStarted(Battle battle)
        {
            OnBattleStarted?.Invoke(battle);
        }
        
        
        public event Action<string> OnFinishBattle;
        public void FinishBattle(string battleId)
        {
            OnFinishBattle?.Invoke(battleId);
        }
        
        public event Action<Battle> OnBattleFinished;
        public void BattleFinished(Battle battle)
        {
            OnBattleFinished?.Invoke(battle);
        }
    }
}