using UnityEngine;
using UnityEngine.Tilemaps;

public class HideTilemapRenderer : MonoBehaviour
{
    private TilemapRenderer tileRenderer;
    void Start()
    {
        tileRenderer = GetComponent<TilemapRenderer>();
        tileRenderer.enabled = false;
    }
}
