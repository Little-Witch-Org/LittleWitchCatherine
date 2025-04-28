using System;

namespace _Scripts.Events
{
    public class PlayerStatsEvents
    {
        
        //---update stat
        public event Action<float> OnUpdateHealth;
        public void UpdateHealth(float health) 
        {
            OnUpdateHealth?.Invoke(health);
        }
        
        public event Action<float> OnUpdateSatiety;
        public void UpdateSatiety(float Satiety) 
        {
            OnUpdateSatiety?.Invoke(Satiety);
        }
        
        
        public event Action<float> OnUpdateMood;
        public void UpdateMood(float mood) 
        {
            OnUpdateMood?.Invoke(mood);
        }
        
        public event Action<float> OnUpdateEnergy;
        public void UpdateEnergy(float energy) 
        {
            OnUpdateEnergy?.Invoke(energy);
        }
        
        
        
        //---stat changed
        public event Action<float> OnHealthChanged;
        public void HealthChanged(float currentHealth) 
        {
            OnHealthChanged?.Invoke(currentHealth);
        }
        
        public event Action<float> OnSatietyChanged;
        public void SatietyChanged(float currentSatiety) 
        {
            OnSatietyChanged?.Invoke(currentSatiety);
        }
        
        public event Action<float> OnMoodChanged;
        public void MoodChanged(float currentMood) 
        {
            OnMoodChanged?.Invoke(currentMood);
        }
        
        public event Action<float> OnEnergyChanged;
        public void EnergyChanged(float currentEnergy) 
        {
            OnEnergyChanged?.Invoke(currentEnergy);
        }
        
        //rate
        public event Action<float,float,float,float> OnStatsChangeRateChanged;
        public void StatsChangeRateChanged(float healthChangeRate, float satietyChangeRate, float moodChangeRate, float energyChangeRate) 
        {
            OnStatsChangeRateChanged?.Invoke(healthChangeRate, satietyChangeRate, moodChangeRate, energyChangeRate );
        }

    }
}