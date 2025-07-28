using System;

namespace _Scripts.Events
{
    public class ReputationEvents
    {
        public event Action<string, float> OnSetReputation;
        public void SetReputation(string npcName, float reputation) 
        {
            OnSetReputation?.Invoke(npcName, reputation);
        }
        
        public event Action<string, float> OnUpdateReputation;
        public void UpdateReputation(string npcName, float reputation) 
        {
            OnUpdateReputation?.Invoke(npcName, reputation);
        }
        
        public event Action<string, float> OnReputationChanged;
        public void ReputationChanged(string npcName, float currentReputation) 
        {
            OnReputationChanged?.Invoke(npcName, currentReputation);
        }
    }
}