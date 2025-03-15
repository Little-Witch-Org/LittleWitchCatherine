using System;
using System.Collections.Generic;
using _Scripts;
using _Scripts.Enums;
using NUnit.Framework;
using UnityEngine;


/// <summary>
/// Stores spawn point objects. Player Spawner gets spawn point position from here
/// </summary>
public class SpawnPointsManager : MonoBehaviour
{
    public static SpawnPointsManager Instance;

    public List<GameObject> spawnPositionObjects;

    private Transform _spawnPointPosition;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    public Transform getSpawnPosition(SpawnPointsNamesCatherineHouseMap spawnPointName)
    {
        //Debug.LogFormat("1 Spawner - get spawn point from PlayerManager = {0}", spawnPointName);

        foreach (var obj in spawnPositionObjects)
        {
            if (obj.name == spawnPointName.ToString())
            {
                _spawnPointPosition =  obj.transform;
            }
        }
        //Debug.LogFormat("returning spawn position from spawn manager {0}",_spawnPointPosition);
        return _spawnPointPosition;
    }
}
