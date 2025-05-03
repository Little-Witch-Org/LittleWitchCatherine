using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Components.Misc
{
    public class MouseInteractionComponent : MonoBehaviour
    {
        private Camera _mainCamera;
        private IClickable _currentHover;
        private Vector3 _lastMousePosition;
        private bool _isMouseDown;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                if (_currentHover != null) ClearHover();
                return;
            }

            HandleHover(); // Всегда проверяем hover, даже без движения

            if (Input.GetMouseButtonDown(0))
            {
                if (_currentHover != null)
                {
                    _isMouseDown = true;
                    _currentHover.OnClick();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_isMouseDown)
                {
                    _isMouseDown = false;

                    if (_currentHover != null)
                    {
                        _currentHover.OnMouseButtonUp();
                    }
                }
            }
        }

        private void HandleHover()
        {
            Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            var newHover = hit.collider != null ? hit.collider.GetComponent<IClickable>() : null;

            if (newHover != _currentHover)
            {
                if (_currentHover != null) _currentHover.OnHoverExit();

                _currentHover = newHover;

                if (_currentHover != null) _currentHover.OnHoverEnter();
            }
        }

        private void ClearHover()
        {
            if (_currentHover != null)
            {
                _currentHover.OnHoverExit();
                _currentHover = null;
            }
        }
    }
}