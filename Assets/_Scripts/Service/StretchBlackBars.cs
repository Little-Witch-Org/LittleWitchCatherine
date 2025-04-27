using UnityEngine;

namespace _Scripts.Service
{
    /// <summary>
    /// Uses for stretch default square sprite (background or fade) in the novel view to screen size. 
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class StretchBlackBars : MonoBehaviour
    {
        private void Start()
        {
            StretchToScreenSize();
        }

        private void StretchToScreenSize()
        {
            float screenHeight = Camera.main.orthographicSize * 2;
            float screenWidth = screenHeight * Camera.main.aspect;
            
            transform.localScale = new Vector3(screenWidth, screenHeight, 1f);
            
            transform.position = new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                0f
            );
        }
    }
}