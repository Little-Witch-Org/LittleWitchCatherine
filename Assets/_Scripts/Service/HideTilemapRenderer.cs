using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.Service
{
    public class HideTilemapRenderer : MonoBehaviour
    {
        private TilemapRenderer tileRenderer;
        void Start()
        {
            tileRenderer = GetComponent<TilemapRenderer>();
            tileRenderer.enabled = false;
        }
    }
}
