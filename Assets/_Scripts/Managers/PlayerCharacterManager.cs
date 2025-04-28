using System;
using System.Diagnostics;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace _Scripts
{
    /// <summary>
    /// Stores Player character game values //todo need to handle exit on map (clear fields)
    /// todo add separated specific class with player stats ?
    /// </summary>
    public class PlayerCharacterManager : MonoBehaviour
    {
        public static PlayerCharacterManager Instance;

        [Header("Transition")] [SerializeField]
        private SceneNames currentMap; // todo scene?

        [SerializeField] private string currentLocation;
        [SerializeField] private string currentNovelPlace;
        [SerializeField] private SpawnPointsNamesCatherineHouseMap storedSpawnPointOnMap;

        [Header("Stats Max")] [SerializeField] private float maxHealth;
        [SerializeField] private float maxSatiety; //hunger
        [SerializeField] private float maxMood;
        [SerializeField] private float maxEnergy;

        [Header("Stats Current")] [SerializeField]
        private float currentHealth;

        [SerializeField] private float currentSatiety;
        [SerializeField] private float currentMood;
        [SerializeField] private float currentEnergy;

        [Header("Stats ChangeRate")] //rate per minute

        [SerializeField]
        private float healthChangeRate;

        [SerializeField] private float satietyChangeRate;
        [SerializeField] private float moodChangeRate;
        [SerializeField] private float energyChangeRate;

        // if we're change stats using time (min's),then we update all types per tick at once (don't recalculate each stat).
        // If we're updating stats for other resources (like eating something or getting damage) -> recalculating change rate for each stat (almost for ui numbers update)
        private bool _isUpdatingStatsBasedOnTime;

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

            InitializeDefaultValues();
        }

        private void OnEnable()
        {
            EventManager.Instance.TransitionEvents.OnCurrentPlaceOnScreen += UpdateCurrentPlaceOn;

            EventManager.Instance.PlayerStatsEvents.OnUpdateHealth += UpdateHeath;
            EventManager.Instance.PlayerStatsEvents.OnUpdateSatiety += UpdateSatiety;
            EventManager.Instance.PlayerStatsEvents.OnUpdateMood += UpdateMood;
            EventManager.Instance.PlayerStatsEvents.OnUpdateEnergy += UpdateEnergy;

            EventManager.Instance.TimeEvents.OnMinutesChanged += UpdateStatsBaseOnMinutes;
            EventManager.Instance.TimeEvents.OnHoursChanged += UpdateStatsBaseOnHours;
        }

        private void OnDisable()
        {
            EventManager.Instance.TransitionEvents.OnLoadedPlace -= UpdateCurrentPlaceOn;

            EventManager.Instance.PlayerStatsEvents.OnUpdateHealth -= UpdateHeath;
            EventManager.Instance.PlayerStatsEvents.OnUpdateSatiety -= UpdateSatiety;
            EventManager.Instance.PlayerStatsEvents.OnUpdateMood -= UpdateMood;
            EventManager.Instance.PlayerStatsEvents.OnUpdateEnergy -= UpdateEnergy;

            EventManager.Instance.TimeEvents.OnMinutesChanged -= UpdateStatsBaseOnMinutes;
            EventManager.Instance.TimeEvents.OnHoursChanged -= UpdateStatsBaseOnHours;
        }

        private void InitializeDefaultValues()
        {
            currentMap = SceneNames.CatherineHouseMap; //todo implement change method for transition in other Maps
            currentLocation = null;
            currentNovelPlace = null;
            storedSpawnPointOnMap = SpawnPointsNamesCatherineHouseMap.StartPositionPoint;

            //stats
            maxHealth = 100;
            currentHealth = maxHealth;
            maxSatiety = 100;
            currentSatiety = 50;
            maxMood = 100;
            currentMood = 50;
            maxEnergy = 100;
            currentEnergy = 70;
            EventManager.Instance.PlayerStatsEvents.HealthChanged(currentHealth);
            EventManager.Instance.PlayerStatsEvents.SatietyChanged(currentSatiety);
            EventManager.Instance.PlayerStatsEvents.MoodChanged(currentMood);
            EventManager.Instance.PlayerStatsEvents.EnergyChanged(currentEnergy);

            Debug.Log("Calculating stats in initialization:");
            CalculateStatsChangeRate();
        }


        public void SetPreviousSpawnPositionPoint(SpawnPointsNamesCatherineHouseMap spawnPoint)
        {
            storedSpawnPointOnMap = spawnPoint;
            //Debug.LogFormat("Point name saved in player char manager = {0}",spawnPoint);
        }

        public SpawnPointsNamesCatherineHouseMap GetPreviousSpawnPositionPoint()
        {
            return storedSpawnPointOnMap;
        }

        private void UpdateCurrentPlaceOn(string location, string place)
        {
            currentNovelPlace = place;
            currentLocation = location;
        }


        public string GetCurrentNovelPlace()
        {
            return currentNovelPlace;
        }

        public string GetCurrentLocation()
        {
            return currentLocation;
        }

        //stats
        private void UpdateHeath(float health)
        {
            var startHealth = currentHealth;
            currentHealth = Mathf.Round((currentHealth + health) * 1000f) / 1000f;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);



            if (!_isUpdatingStatsBasedOnTime)
            {
                EventManager.Instance.PlayerStatsEvents.HealthChanged(currentHealth);
                Debug.Log($"Health updated {startHealth} -> {currentHealth} ({health}).");
                CalculateStatsChangeRate();
            }
        }

        private void UpdateSatiety(float satiety)
        {
            var startSatiety = currentSatiety;
            currentSatiety = Mathf.Round((currentSatiety + satiety) * 1000f) / 1000f;
            currentSatiety = Mathf.Clamp(currentSatiety, 0, maxSatiety);


            if (!_isUpdatingStatsBasedOnTime)
            {
                EventManager.Instance.PlayerStatsEvents.SatietyChanged(currentSatiety);
                Debug.Log($"Satiety updated {startSatiety} -> {currentSatiety} ({satiety}).");
                CalculateStatsChangeRate();
            }
        }

        private void UpdateMood(float mood)
        {
            var startMood = currentMood;
            currentMood = Mathf.Round((currentMood + mood) * 1000f) / 1000f;
            currentMood = Mathf.Clamp(currentMood, 0, maxMood);


            if (!_isUpdatingStatsBasedOnTime)
            {
                EventManager.Instance.PlayerStatsEvents.MoodChanged(currentMood);
                Debug.Log($"Mood updated {startMood} -> {currentMood} ({mood}).");
                CalculateStatsChangeRate();
            }
        }

        private void UpdateEnergy(float energy)
        {
            var startEnergy = currentEnergy;
            currentEnergy = Mathf.Round((currentEnergy + energy) * 1000f) / 1000f;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);


            if (!_isUpdatingStatsBasedOnTime)
            {
                EventManager.Instance.PlayerStatsEvents.EnergyChanged(currentEnergy);
                Debug.Log($"Energy updated {startEnergy} -> {currentEnergy} ({energy}).");
                CalculateStatsChangeRate();
            }
        }



        private void UpdateStatsBaseOnMinutes(int minutes)
        {
            _isUpdatingStatsBasedOnTime = true;

            // Сохраняем начальные значения
            float startHealth = currentHealth;
            float startSatiety = currentSatiety;
            float startMood = currentMood;
            float startEnergy = currentEnergy;

            string timePassedString = "Skipping " + minutes + " minutes:";
            string healthString = $"Health changed from {startHealth}";
            string satietyString = $"Satiety changed from {startSatiety}";
            string moodString = $"Mood changed from {startMood}";
            string energyString = $"Energy changed from {startEnergy}";

            string healthChangeProgressionString = "";
            string satietyChangeProgressionString = "";
            string moodChangeProgressionString = "";
            string energyChangeProgressionString = "";

            for (var i = 0; i < minutes; i++)
            {
                CalculateStatsChangeRate();
                UpdateHeath(healthChangeRate);
                UpdateSatiety(satietyChangeRate);
                UpdateMood(moodChangeRate);
                UpdateEnergy(energyChangeRate);

                healthChangeProgressionString += $"{healthChangeRate}";
                satietyChangeProgressionString += $"{satietyChangeRate}";
                moodChangeProgressionString += $"{moodChangeRate}";
                energyChangeProgressionString += $"{energyChangeRate}";

                if (i < minutes - 1)
                {
                    healthChangeProgressionString += " -> ";
                    satietyChangeProgressionString += " -> ";
                    moodChangeProgressionString += " -> ";
                    energyChangeProgressionString += " -> ";
                }
            }
            
            float totalHealthChange = currentHealth - startHealth;
            float totalSatietyChange = currentSatiety - startSatiety;
            float totalMoodChange = currentMood - startMood;
            float totalEnergyChange = currentEnergy - startEnergy;

            healthString += $" to {currentHealth} (Δ{totalHealthChange:+0.00;-0.00}) | -> " +
                            healthChangeProgressionString;
            satietyString += $" to {currentSatiety} (Δ{totalSatietyChange:+0.00;-0.00}) | -> " +
                             satietyChangeProgressionString;
            moodString += $" to {currentMood} (Δ{totalMoodChange:+0.00;-0.00}) | -> " + moodChangeProgressionString;
            energyString += $" to {currentEnergy} (Δ{totalEnergyChange:+0.00;-0.00}) | -> " +
                            energyChangeProgressionString;

            Debug.Log(timePassedString);
            Debug.Log(healthString);
            Debug.Log(satietyString);
            Debug.Log(moodString);
            Debug.Log(energyString);

            _isUpdatingStatsBasedOnTime = false;
            Debug.Log("Calculating once after loop:");
            CalculateStatsChangeRate();
            
            EventManager.Instance.PlayerStatsEvents.HealthChanged(currentHealth);
            EventManager.Instance.PlayerStatsEvents.SatietyChanged(currentSatiety);
            EventManager.Instance.PlayerStatsEvents.MoodChanged(currentMood);
            EventManager.Instance.PlayerStatsEvents.EnergyChanged(currentEnergy);
        }

        private void UpdateStatsBaseOnHours(int hours)
        {
            UpdateStatsBaseOnMinutes(hours * 60);
        }

        private void CalculateStatsChangeRate()
        {
            // local vars
            float healthRateFromHealth = 0f;
            float satietyRateFromHealth = 0f;
            float moodRateFromHealth = 0f;
            float energyRateFromHealth = 0f;

            float healthRateFromSatiety = 0f;
            float satietyRateFromSatiety = 0f;
            float moodRateFromSatiety = 0f;
            float energyRateFromSatiety = 0f;

            float healthRateFromMood = 0f;
            float satietyRateFromMood = 0f;
            float moodRateFromMood = 0f;
            float energyRateFromMood = 0f;

            float healthRateFromEnergy = 0f;
            float satietyRateFromEnergy = 0f;
            float moodRateFromEnergy = 0f;
            float energyRateFromEnergy = 0f;

            // Health Switch
            switch (currentHealth)
            {
                case var n when n >= 95 && n <= 100:
                    break;
                case var n when n >= 85 && n < 95:
                    break;
                case var n when n >= 50 && n < 85:
                    break;
                case var n when n >= 35 && n < 50:
                    break;
                case var n when n >= 15 && n < 35:
                    break;
                case var n when n >= 0 && n < 5:
                    break;
                default:
                    break;
            }

            // Satiety Switch
            switch (currentSatiety)
            {
                case var n when n >= 95 && n <= 100:
                    healthRateFromSatiety = -0.03f;
                    break;
                case var n when n >= 85 && n < 95:
                    healthRateFromSatiety = 0.06f;
                    energyRateFromSatiety = 1f;
                    break;
                case var n when n >= 50 && n < 85:
                    healthRateFromSatiety = 0.04f;
                    energyRateFromSatiety = 3f;
                    break;
                case var n when n >= 35 && n < 50:
                    healthRateFromSatiety = 0.03f;
                    energyRateFromSatiety = 2f;
                    break;
                case var n when n >= 15 && n < 35:
                    energyRateFromSatiety = 1f;
                    break;
                case var n when n >= 0 && n < 5:
                    healthRateFromSatiety = -0.09f;
                    break;
                default:
                    break;
            }

            // Mood Switch
            switch (currentMood)
            {
                case var n when n >= 95 && n <= 100:
                    break;
                case var n when n >= 85 && n < 95:
                    break;
                case var n when n >= 50 && n < 85:
                    break;
                case var n when n >= 35 && n < 50:
                    break;
                case var n when n >= 15 && n < 35:
                    break;
                case var n when n >= 0 && n < 5:
                    break;
                default:
                    break;
            }

            // Energy Switch
            switch (currentEnergy)
            {
                case var n when n >= 95 && n <= 100:
                    break;
                case var n when n >= 85 && n < 95:
                    break;
                case var n when n >= 50 && n < 85:
                    break;
                case var n when n >= 35 && n < 50:
                    break;
                case var n when n >= 15 && n < 35:
                    break;
                case var n when n >= 0 && n < 5:
                    break;
                default:
                    break;
            }

            // Rate multiplication
            healthChangeRate = healthRateFromHealth + healthRateFromSatiety + healthRateFromMood + healthRateFromEnergy;
            satietyChangeRate = satietyRateFromHealth + satietyRateFromSatiety + satietyRateFromMood +
                                satietyRateFromEnergy;
            moodChangeRate = moodRateFromHealth + moodRateFromSatiety + moodRateFromMood + moodRateFromEnergy;
            energyChangeRate = energyRateFromHealth + energyRateFromSatiety + energyRateFromMood + energyRateFromEnergy;

            EventManager.Instance.PlayerStatsEvents.StatsChangeRateChanged(healthChangeRate, satietyChangeRate,
                moodChangeRate, energyChangeRate);

            if (!_isUpdatingStatsBasedOnTime)
            {
                Debug.Log(
                    $"[CalculateStatsChangeRate] HealthRate: {healthChangeRate}, SatietyRate: {satietyChangeRate}, MoodRate: {moodChangeRate}, EnergyRate: {energyChangeRate}");
            }
        }


    }

}