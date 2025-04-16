using System;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    /// <summary>
    /// Stores Player character game values //todo need to handle exit on map (clear fields)
    /// </summary>
    public class PlayerCharacterManager : MonoBehaviour
    {
        public static PlayerCharacterManager Instance;
        
        [SerializeField] private SceneNames currentMap; // todo scene?
        [SerializeField] private string currentLocation;
        [SerializeField] private string currentNovelPlace;
        [SerializeField] private SpawnPointsNamesCatherineHouseMap storedSpawnPointOnMap;
        
        
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
        }

        private void OnDisable()
        {
            EventManager.Instance.TransitionEvents.OnLoadedPlace -= UpdateCurrentPlaceOn;
        }

        private void InitializeDefaultValues()
        {
            currentMap = SceneNames.CatherineHouseMap; //todo implement change method for transition in other Maps
            currentLocation = null; 
            currentNovelPlace = null; 
            storedSpawnPointOnMap = SpawnPointsNamesCatherineHouseMap.StartPositionPoint;
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
    }
}