using _Scripts.Managers;
using UnityEngine;


namespace _Scripts.Components.TransitionComponents
{
    /// <summary>
    /// Get transition info from location manager and instantiate Place prefab
    /// Invoke SetLoadedPlaceName event after prefab loaded (uses by Location manager)
    /// </summary>
    public class NovelViewChangePlaceAfterTransitionComponent : MonoBehaviour
    {
        public void ChangeView() //get InfoSo directly from Location Manager
        {
            string locationName = LocationManager.Instance.GetTransitionLocation();
            string placeName = LocationManager.Instance.GetTransitionPlace();
            
            //Debug.Log($"NovelViewPlaces/{locationName}/{placeName}");
            var locationPrefab = UnityEngine.Resources.Load($"NovelViewPlaces/{locationName}/{placeName}") as GameObject;
            Instantiate(locationPrefab);
            
            EventManager.Instance.TransitionEvents.SetLoadedPlaceName(placeName);
            EventManager.Instance.TransitionEvents.SetLoadedLocationName(locationName);
        }

    
    }
}
