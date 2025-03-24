using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Components.SceneTransitionComponents
{
    /// <summary>
    /// Setting the place to transit using event system //todo to the Location manager
    /// </summary>
    public class NovelViewSetPlaceToTransitionComponent : MonoBehaviour
    {
    
        [FormerlySerializedAs("novelViewPlaceNameToTransfer")] [SerializeField] private CatherineHouseNovelViewPlaces novelViewPlaceToTransfer;

        public void SetScenePrefsData()
        {
            PlayerPrefs.SetString("PlaceName", novelViewPlaceToTransfer.ToString());
            //EventManager.Instance.transitionEvents.OnPlaceTransitionTriggered(novelViewPlaceToTransfer.ToString());
        }
    }
}
