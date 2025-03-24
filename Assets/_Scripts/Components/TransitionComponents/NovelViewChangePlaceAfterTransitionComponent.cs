using _Scripts.Managers;
using UnityEngine;



namespace _Scripts.Components.TransitionComponents
{
    /// <summary>
    /// Get transition load info from location manager
    /// </summary>
    public class NovelViewChangePlaceAfterTransitionComponent : MonoBehaviour
    {
        public void ChangeView() //get info directly from Location Manager
        {
            string locationName = LocationManager.Instance.GetTransitionLocation();
            string placeName = LocationManager.Instance.GetTransitionPlace();
            
            //Debug.Log($"NovelViewPlaces/{locationName}/{placeName}");
            var locationPrefab = Resources.Load($"NovelViewPlaces/{locationName}/{placeName}") as GameObject;
            Instantiate(locationPrefab);
        }

    
    }
}
