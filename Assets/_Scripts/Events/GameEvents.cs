using System;

namespace _Scripts.Events
{
    public class GameEvents
    {
        public Action<int> OnActivateCheckpoint;
        public void ActivateCheckpoint(int checkpointID)
        {
            OnActivateCheckpoint?.Invoke(checkpointID);
        }
        public Action<int> OnCheckpointActivated;
        public void CheckpointActivated(int checkpointID)
        {
            OnCheckpointActivated?.Invoke(checkpointID);
        }
        
        public Action<bool> OnStoryModActivated;
        public void StoryModActivated(bool isInStoryMod)
        {
            OnStoryModActivated?.Invoke(isInStoryMod);
        }
    }
}