using System;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    /// <summary>
    /// Component is used to spawn player, store previous position.  todo dont like realization, refactor with script obj\stateEnumTest machine ? need to handle spawn in quest view
    /// </summary>
    public class PlayerSpawnerManager : MonoBehaviour
    {
        //[SerializeField] private Transform previousSpawnPointPosition;
        public static PlayerSpawnerManager Instance { get; private set; }
    
        //public bool IsFirstInstance { get; private set; } = true;
        public SpawnComponent SpawnComponent { get; private set; }
        public GameObject CharacterPrefab { get; private set; }
        
        public SpawnPointsNamesCatherineHouseMap previousSpawnPointName;
        
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoadedSpawnPlayer;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoadedSpawnPlayer;
        }
        


        
        private void Start()
        {
            //Debug.LogFormat("Current spawn point on Start: {0}", previousSpawnPointName);
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                //Debug.Log("Instance already exists!");
                return;  //handle double load scene for second copy of script
            }
            
            //Spawn character only after scene loaded and scene check (in method)
            Scene currentScene = SceneManager.GetActiveScene();
            OnSceneLoadedSpawnPlayer(currentScene, LoadSceneMode.Single); //SceneManager.sceneLoaded needs two parameters
            
        }

        //todo char spawn in other (map) scenes need to be handled
        private void OnSceneLoadedSpawnPlayer(Scene scene, LoadSceneMode mode)
        {
            if (Instance != this) return; //handle double spawn (spawn only for first copy of this script (Instance))
            
            if (scene.name == SceneNames.CatherineHouseMap.ToString()|| scene.name.Contains("Test"))
            {
                //Debug.LogFormat("Scene loaded: {0}. Current spawn point: {1}", scene.name, previousSpawnPointName);
                SpawnCharacter();
                //Debug.LogFormat("Spawned in {0}", previousSpawnPointName);

            }
        }

        private void SpawnCharacter()
        {
            previousSpawnPointName = PlayerCharacterManager.Instance.GetPreviousSpawnPositionPoint();
            //Debug.LogFormat("Prepare to spawn Player to {0}", previousSpawnPointName);
            CharacterPrefab = Resources.Load("Witch") as GameObject;
            SpawnComponent = GetComponent<SpawnComponent>();
            SpawnComponent.Spawn(CharacterPrefab, SpawnPointsManager.Instance.getSpawnPosition(previousSpawnPointName).position);
        }
    }
}
