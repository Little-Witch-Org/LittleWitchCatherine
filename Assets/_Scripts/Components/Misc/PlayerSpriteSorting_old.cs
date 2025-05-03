using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Components.Misc
{
    public class PlayerSpriteSorting_old : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer playerRenderer;

        [Header("Sorting Settings")] [SerializeField, Tooltip("Order when player is in front of objects")]
        private int defaultOrder = 1;

        [SerializeField, Tooltip("Order when player is behind objects")]
        private int behindOrder = -1;

        [SerializeField, Tooltip("Sorting layer name to check for dynamic objects")]
        private string targetSortingLayer = "DynamicMapObjects";

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!ShouldProcessCollision(other, out var objectRenderer)) return;

            bool isPlayerBelow = playerRenderer.bounds.min.y > objectRenderer.bounds.min.y;
            playerRenderer.sortingOrder = isPlayerBelow ? behindOrder : defaultOrder;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!ShouldProcessCollision(other, out _)) return;
            playerRenderer.sortingOrder = defaultOrder;
        }

        private bool ShouldProcessCollision(Collider2D other, out SpriteRenderer renderer)
        {
            renderer = null;
            return other != null &&
                   other.TryGetComponent(out renderer) &&
                   renderer.sortingLayerName == targetSortingLayer;
        }
    }
}