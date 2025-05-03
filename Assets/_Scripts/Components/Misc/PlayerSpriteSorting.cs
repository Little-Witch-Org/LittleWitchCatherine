using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Components.Misc
{
    /// <summary>
    /// Manages player's sorting order depending on overlapping transparent objects.
    /// </summary>
    public class PlayerSpriteSorting : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer playerRenderer;

        [Header("Sorting Settings")]
        [SerializeField] private int defaultOrder = 2000;

        // Активные коллайдеры, в которые сейчас входит игрок
        private readonly List<SpriteRenderer> _activeRenderers = new();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out SpriteRenderer objectRenderer))
            {
                //Debug.Log(objectRenderer.sortingLayerName);
                if (!_activeRenderers.Contains(objectRenderer) && objectRenderer.sortingLayerName.Equals("DynamicMapObjects")) //handle only IFadable
                    _activeRenderers.Add(objectRenderer);

                UpdateSortingOrder();
                //Debug.Log(_activeRenderers.Count);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out SpriteRenderer objectRenderer) && objectRenderer.sortingLayerName.Equals("DynamicMapObjects"))
            {
                if (_activeRenderers.Contains(objectRenderer))
                    _activeRenderers.Remove(objectRenderer);

                UpdateSortingOrder();
            }
        }

        private void UpdateSortingOrder()
        {
            if (_activeRenderers.Count == 0)
            {
                playerRenderer.sortingOrder = defaultOrder;
            }
            else
            {
                int minOrder = _activeRenderers.Min(r => r.sortingOrder);
                playerRenderer.sortingOrder = minOrder - 1;
            }
        }
    }
}