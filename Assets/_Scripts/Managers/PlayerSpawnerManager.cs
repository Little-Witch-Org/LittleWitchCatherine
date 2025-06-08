using _Scripts.Components.SpawnComponents;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers
{
    /// <summary>
    /// Component is used to spawn player, store previous position.  todo dont like realization, refactor with script obj\stateEnum machine ? need to handle spawn in quest view
    /// </summary>
    public class PlayerSpawnerManager : MonoBehaviour
    {
        //[SerializeField] private Transform previousSpawnPointPosition;
        public static PlayerSpawnerManager Instance { get; private set; }
    
        //public bool IsFirstInstance { get; private set; } = true;
        public SpawnComponent SpawnComponent { get; private set; }
        public GameObject CharacterPrefab { get; private set; }
        
        public SpawnPointsNamesCatherineHouseMapEnum previousSpawnPointName;
        
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
            
            //Spawn character after scene loaded (for first load in redactor/build) 
            Scene currentScene = SceneManager.GetActiveScene();
            TrySpawnCharacter(currentScene.name);
            //OnSceneLoadedSpawnPlayer(currentScene, LoadSceneMode.Single); //SceneManager.sceneLoaded needs two parameters
            
        }
        
        //handle double spawn
        private void TrySpawnCharacter(string sceneName)
        {
            if (GameObject.FindWithTag("Player") != null) return;

            if (sceneName == SceneNamesEnum.CatherineHouseMapScene.ToString() 
                || sceneName.Contains("Test") 
                || sceneName.Contains("NewMapTemp"))
            {
                SpawnCharacter(sceneName);
            }
        }

        //todo char spawn in other (map) scenes need to be handled
        private void OnSceneLoadedSpawnPlayer(Scene scene, LoadSceneMode mode)
        {
            if (Instance != this) return; //handle double spawn
            
            if (scene.name == SceneNamesEnum.CatherineHouseMapScene.ToString()|| scene.name.Contains("Test")|| scene.name.Contains("NewMapTemp"))
            {
                //Debug.LogFormat("Scene loaded: {0}. Current spawn point: {1}", scene.name, previousSpawnPointName);
                SpawnCharacter(scene.name);
                //Debug.LogFormat("Spawned in {0}", previousSpawnPointName);

            }
        }

        private void SpawnCharacter(string sceneName)
        {
            previousSpawnPointName = PlayerCharacterManager.Instance.GetPreviousSpawnPositionPoint();
            //Debug.LogFormat("Prepare to spawn Player to {0}", previousSpawnPointName);
            if (sceneName == "CatherineHouseMapScene")
            {
                CharacterPrefab = UnityEngine.Resources.Load("Prefabs/_Player/Witch2") as GameObject;
            }
            else
            {
                CharacterPrefab = UnityEngine.Resources.Load("Prefabs/_Player/Witch") as GameObject;
            }

            SpawnComponent = GetComponent<SpawnComponent>();
            SpawnComponent.Spawn(CharacterPrefab, SpawnPointsManager.Instance.getSpawnPosition(previousSpawnPointName).position);
        }
        
    }
}
