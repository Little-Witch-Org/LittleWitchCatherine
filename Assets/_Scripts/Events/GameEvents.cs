using System;

namespace _Scripts.Events
{
    public class GameEvents
    {
        public Action<int> OnCheckpointActivated;
        public void CheckpointActivated(int checkpointID)
        {
            OnCheckpointActivated?.Invoke(checkpointID);
        }
    }
}