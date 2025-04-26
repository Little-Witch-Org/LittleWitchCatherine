using System;

namespace _Scripts.Events
{
    public class PlayerStatsEvents
    {
        public event Action<int> OnUpdateHealth;
        public void UpdateHealth(int health) 
        {
            OnUpdateHealth?.Invoke(health);
        }
        
        public event Action<int> OnUpdateSaturation;
        public void UpdateSaturation(int saturation) 
        {
            OnUpdateSaturation?.Invoke(saturation);
        }
        
        
        public event Action<int> OnUpdateMood;
        public void UpdateMood(int mood) 
        {
            OnUpdateMood?.Invoke(mood);
        }
        
        public event Action<int> OnUpdateEnergy;
        public void UpdateEnergy(int energy) 
        {
            OnUpdateEnergy?.Invoke(energy);
        }
        
        
        
        
        public event Action<int> OnHealthChanged;
        public void HealthChanged(int currentHealth) 
        {
            OnHealthChanged?.Invoke(currentHealth);
        }
        
        public event Action<int> OnSaturationChanged;
        public void SaturationChanged(int currentSaturation) 
        {
            OnSaturationChanged?.Invoke(currentSaturation);
        }
        
        public event Action<int> OnMoodChanged;
        public void MoodChanged(int currentMood) 
        {
            OnMoodChanged?.Invoke(currentMood);
        }
        
        public event Action<int> OnEnergyChanged;
        public void EnergyChanged(int currentEnergy) 
        {
            OnEnergyChanged?.Invoke(currentEnergy);
        }

    }
}