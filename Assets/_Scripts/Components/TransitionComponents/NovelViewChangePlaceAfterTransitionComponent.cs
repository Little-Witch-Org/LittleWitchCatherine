using _Scripts.Managers;
using UnityEngine;


namespace _Scripts.Components.TransitionComponents
{
    /// <summary>
    /// Get transition info from location manager and instantiate Place prefab //todo use events instead of get from manager ?
    /// Invoke LoadedPlace event after prefab loaded (uses by Location manager)
    /// </summary>
    public class NovelViewChangePlaceAfterTransitionComponent : MonoBehaviour
    {
        public void ChangeView() //get InfoSo directly from Location Manager
        {
            string mapName = "CatherineHouseMap";
            string locationName = LocationManager.Instance.GetTransitionLocation();
            string placeName = LocationManager.Instance.GetTransitionPlace();
            
            //Debug.Log($"Prefabs/NovelViewPlaces/{mapName}/{locationName}/{placeName}");
            var locationPrefab = UnityEngine.Resources.Load($"Prefabs/NovelViewPlaces/{mapName}/{locationName}/{placeName}") as GameObject;
            Instantiate(locationPrefab);
            
            //Debug.Log("invoke loaded location and place "+ locationName +" "+placeName);
            EventManager.Instance.TransitionEvents.LoadedPlace(locationName, placeName);
        }

    
    }
}
