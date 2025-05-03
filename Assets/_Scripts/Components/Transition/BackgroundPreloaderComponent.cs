using UnityEngine;

namespace _Scripts.Components.Transition
{
    public class BackgroundPreloaderComponent : MonoBehaviour
    {

        void Start()
        {
            
                // Загружаем спрайт в память
                Sprite[] allSprites = UnityEngine.Resources.LoadAll<Sprite>("Art/MapsLocationPlacesSprites/NovelViewPlacesSprites");
            
                Debug.Log($"Загружено {allSprites.Length} спрайтов");
            
        }
    }
}