using System;

namespace _Scripts.events_test
{
    public class ExpEvents_Test
    {
        public event Action<int> OnExperienceGained;
        public void ExperienceGained(int experience) 
        {
           OnExperienceGained?.Invoke(experience);
        }
        

        public event Action<int> OnPlayerExperienceChange;
        public void PlayerExperienceChange(int experience) 
        {
          OnPlayerExperienceChange?.Invoke(experience);
        }
        
        public event Action<int> OnPlayerLevelChange;
        public void PlayerLevelChange(int lvl) 
        {
            OnPlayerLevelChange?.Invoke(lvl);
        }
    }
}