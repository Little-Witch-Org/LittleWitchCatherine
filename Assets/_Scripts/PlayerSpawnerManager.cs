using System;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    /// <summary>
    /// Component is used to spawn player, store previous position.  todo dont like realization, refactor with script obj\state machine ? need to handle spawn in quest view
    /// </summary>
    public class PlayerSpawnerManager : MonoBehaviour
    {
        //[SerializeField] private Transform previousSpawnPointPosition;
        public static PlayerSpawnerManager Instance { get; private set; }
    
        public bool IsFirstInstance { get; private set; } = true;
        public SpawnComponent spawnComponent { get; private set; }
        public GameObject CharacterPrefab { get; private set; }
        
        public SpawnPointsNamesCatherineHouseMap previousSpawnPointName;
        
        public bool isInitialSceneHandled;
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
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
            
            //handle load from location scene
            //Debug.Log("code block");
            if (!isInitialSceneHandled)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                OnSceneLoaded(currentScene, LoadSceneMode.Single);
                isInitialSceneHandled = true;
            }
            
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Instance != this) return; //handle double spawn (spawn only for first copy of this script (Instance))
            
            if (scene.name == SceneNames.CatherineHouseMap.ToString())
            {
                //Debug.LogFormat("Scene loaded: {0}. Current spawn point: {1}", scene.name, previousSpawnPointName);
                SpawnCharacter();
                //Debug.LogFormat("Spawned in {0}", previousSpawnPointName);

            }
        }

        public void SetPreviousSpawnPositionPoint(SpawnPointsNamesCatherineHouseMap spawnPoint)
        {
            previousSpawnPointName = spawnPoint;
            //Debug.LogFormat("Point name saved in PlayerSpawnerManager = {0}",spawnPoint);
        }

        private void SpawnCharacter()
        {
            //Debug.LogFormat("Prepare to spawn Player to {0}", previousSpawnPointName);
            CharacterPrefab = Resources.Load("Witch") as GameObject;
            spawnComponent = GetComponent<SpawnComponent>();
            spawnComponent.Spawn(CharacterPrefab, SpawnPointsManager.Instance.getSpawnPosition(previousSpawnPointName).position);
        }
    }
}
