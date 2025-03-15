using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class QuestViewChangerComponent : MonoBehaviour
{
    public void ChangeView()
    {
        var initialSceneName = PlayerPrefs.GetString("LocationName");
        var locationPrefab = Resources.Load($"QuestViewLocation/{initialSceneName}") as GameObject;
        Instantiate(locationPrefab);
    }

    
}
