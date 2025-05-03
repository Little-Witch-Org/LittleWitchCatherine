using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Service
{
    public class DynamicCameraAdjuster : MonoBehaviour
    {
    
        [Header("Настройки")]
        [Tooltip("Если включено, камера будет обрезать фон по бокам (чтобы не было черных полос сверху/снизу)")]
        [SerializeField] private bool cropToFitHeight = true;

        private Camera _camera;
        private SpriteRenderer _currentBackground;

        private void Awake()
        {
            _camera = Camera.main;
        
            // Подписываемся на событие появления нового фона
            EventManager.Instance.TransitionEvents.OnPlaceSpriteChanged += AdjustToNewBackground;
        }

        private void OnDestroy()
        {
            // Отписываемся при уничтожении объекта
            EventManager.Instance.TransitionEvents.OnPlaceSpriteChanged -= AdjustToNewBackground;
        }

        // Вызывается при появлении нового фона
        private void AdjustToNewBackground(SpriteRenderer newBackground)
        {
            _currentBackground = newBackground;
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            if (_currentBackground == null) return;

            float bgHeight = _currentBackground.bounds.size.y;
            float bgWidth = _currentBackground.bounds.size.x;
            float bgAspect = bgWidth / bgHeight;
            float screenAspect = (float)Screen.width / Screen.height;

            // Режим 1: Подгоняем камеру под высоту фона (обрезаем по бокам)
            if (cropToFitHeight)
            {
                _camera.orthographicSize = bgHeight / 2;
            }
            // Режим 2: Подгоняем камеру под ширину фона (обрезаем сверху/снизу)
            else
            {
                _camera.orthographicSize = bgWidth / (2 * screenAspect);
            }

            // Центрируем камеру на фоне
            _camera.transform.position = new Vector3(
                _currentBackground.transform.position.x,
                _currentBackground.transform.position.y,
                _camera.transform.position.z
            );
        }
    }
}