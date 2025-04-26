using _Scripts.Enums;
using UnityEngine;

namespace _Scripts.Components.SpawnComponents
{
    /// <summary>
    /// Enter trigger sets previous spawn point to Player Char Manager. (uses for future spawn when the character returns from novel view)
    /// Added component on location exit points (to handle teleport into location without using current entrance)
    /// </summary>
    public class StorePreviousPositionPointComponent : MonoBehaviour
    {


        [SerializeField] private SpawnPointsNamesCatherineHouseMap pointNamesName;


        public void SetPreviousPositionPointName()
        {
            PlayerCharacterManager.Instance.SetPreviousSpawnPositionPoint(pointNamesName); 
            //Debug.LogFormat("Saved point name from trigger = {0}", pointNamesName);
        }
    }
}