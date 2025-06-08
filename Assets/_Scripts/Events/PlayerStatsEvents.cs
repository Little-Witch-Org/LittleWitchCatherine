using System;

namespace _Scripts.Events
{
    /// <summary>
    /// {1} Update {1} -> {2} OnUpdate / Changed {2} -> {3} OnChanged {3} //todo change other events using this style
    /// </summary>
    public class PlayerStatsEvents
    {
        
        //---update stat
        public event Action<float> OnHealthUpdate;
        public void UpdateHealth(float health) 
        {
            OnHealthUpdate?.Invoke(health);
        }
        
        public event Action<float> OnSatietyUpdate;
        public void UpdateSatiety(float Satiety) 
        {
            OnSatietyUpdate?.Invoke(Satiety);
        }
        
        
        public event Action<float> OnMoodUpdate;
        public void UpdateMood(float mood) 
        {
            OnMoodUpdate?.Invoke(mood);
        }
        
        public event Action<float> OnEnergyUpdate;
        public void UpdateEnergy(float energy) 
        {
            OnEnergyUpdate?.Invoke(energy);
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