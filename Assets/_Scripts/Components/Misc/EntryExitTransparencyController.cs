using UnityEngine;

namespace _Scripts.Components.Misc
{
    /// <summary>
    /// Used on player prefab. Triggers IFadeable (sprites on map)
    /// </summary>
    public class EntryExitTransparencyController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IFadeable>(out IFadeable fadeableObject))
            {
                fadeableObject.FadeIn();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IFadeable>(out IFadeable fadeableObject))
            {
                fadeableObject.FadeOut();
            }
        }
    }
}
