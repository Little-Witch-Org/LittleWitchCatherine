using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class NovelViewLocationChangerComponent : MonoBehaviour
{
    public void ChangeView()
    {
        var initialSceneName = PlayerPrefs.GetString("LocationName");
        var locationPrefab = Resources.Load($"NovelViewLocation/{initialSceneName}") as GameObject;
        Instantiate(locationPrefab);
    }

    
}
