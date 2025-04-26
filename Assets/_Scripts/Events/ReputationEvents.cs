using System;

namespace _Scripts.Events
{
    public class ReputationEvents
    {
        public event Action<string, int> OnUpdateReputation;
        public void UpdateReputation(string npcName, int reputation) 
        {
            OnUpdateReputation?.Invoke(npcName, reputation);
        }
        
        public event Action<string, int> OnReputationChanged;
        public void ReputationChanged(string npcName, int currentReputation) 
        {
            OnReputationChanged?.Invoke(npcName, currentReputation);
        }
    }
}