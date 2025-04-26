using System;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    /// <summary>
    /// Stores Player character game values //todo need to handle exit on map (clear fields)
    /// todo add class with player stats ?
    /// </summary>
    public class PlayerCharacterManager : MonoBehaviour
    {
        public static PlayerCharacterManager Instance;
        
        [Header("Transition")]
        [SerializeField] private SceneNames currentMap; // todo scene?
        [SerializeField] private string currentLocation;
        [SerializeField] private string currentNovelPlace;
        [SerializeField] private SpawnPointsNamesCatherineHouseMap storedSpawnPointOnMap;
        
        [Header("Stats")]
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxSaturation; //hunger
        [SerializeField] private int currentSaturation; //hunger
        [SerializeField] private int maxMood;
        [SerializeField] private int currentMood;
        [SerializeField] private int maxEnergy;
        [SerializeField] private int currentEnergy;
        
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
            EventManager.Instance.PlayerStatsEvents.OnUpdateSaturation += UpdateSaturation;
            EventManager.Instance.PlayerStatsEvents.OnUpdateMood += UpdateMood;
            EventManager.Instance.PlayerStatsEvents.OnUpdateEnergy += UpdateEnergy;
        }

        private void OnDisable()
        {
            EventManager.Instance.TransitionEvents.OnLoadedPlace -= UpdateCurrentPlaceOn;
            
            EventManager.Instance.PlayerStatsEvents.OnUpdateHealth -= UpdateHeath;
            EventManager.Instance.PlayerStatsEvents.OnUpdateSaturation -= UpdateSaturation;
            EventManager.Instance.PlayerStatsEvents.OnUpdateMood -= UpdateMood;
            EventManager.Instance.PlayerStatsEvents.OnUpdateEnergy -= UpdateEnergy;
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
            maxSaturation = 100;
            currentSaturation = 50;
            maxMood = 100;
            currentMood = 50;
            maxEnergy = 100;
            currentEnergy = 70;
            EventManager.Instance.PlayerStatsEvents.HealthChanged(currentHealth);
            EventManager.Instance.PlayerStatsEvents.SaturationChanged(currentSaturation);
            EventManager.Instance.PlayerStatsEvents.MoodChanged(currentMood);
            EventManager.Instance.PlayerStatsEvents.EnergyChanged(currentEnergy);
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
        private void UpdateHeath(int health)
        {
            currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);
            
            EventManager.Instance.PlayerStatsEvents.HealthChanged(currentHealth);
        }
        
        private void UpdateSaturation(int saturation)
        {
            currentSaturation = Math.Clamp(currentSaturation + saturation, 0, maxSaturation);
            EventManager.Instance.PlayerStatsEvents.SaturationChanged(currentSaturation);
        }
        
        private void UpdateMood(int mood)
        {
            currentMood = Math.Clamp(currentMood + mood, 0, maxMood);
            EventManager.Instance.PlayerStatsEvents.MoodChanged(currentMood);
        }
        
        private void UpdateEnergy(int energy)
        {
            currentEnergy = Math.Clamp(currentEnergy + energy, 0, maxEnergy);
            EventManager.Instance.PlayerStatsEvents.EnergyChanged(currentEnergy);
        }
        
    }
}