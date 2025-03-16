using _Scripts;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Enter trigger sets spawn point to SpawnPointsManager using this component (uses for future spawn when the character returns from quest view)
/// </summary>
public class StorePreviousPositionPointComponent : MonoBehaviour
{


    [SerializeField] private SpawnPointsNamesCatherineHouseMap pointNamesName;


    public void setPreviousPositionPointNameToPlayerSpawner()
    {
        PlayerSpawnerManager.Instance.SetPreviousSpawnPositionPoint(pointNamesName);
        //Debug.LogFormat("Saved point name from trigger = {0}", pointNamesName);
    }
}