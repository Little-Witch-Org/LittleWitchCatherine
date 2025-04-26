using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Components.Misc
{
    public class MouseInteractionComponent : MonoBehaviour
    {
        private Camera mainCamera;
        private IClickable currentHover;
        private Vector3 lastMousePosition;
        private bool isMouseDown;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                if (currentHover != null) ClearHover();
                return;
            }

            HandleHover(); // Всегда проверяем hover, даже без движения

            if (Input.GetMouseButtonDown(0))
            {
                if (currentHover != null)
                {
                    isMouseDown = true;
                    currentHover.OnClick();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isMouseDown)
                {
                    isMouseDown = false;

                    if (currentHover != null)
                    {
                        currentHover.OnMouseButtonUp();
                    }
                }
            }
        }

        private void HandleHover()
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            var newHover = hit.collider != null ? hit.collider.GetComponent<IClickable>() : null;

            if (newHover != currentHover)
            {
                if (currentHover != null) currentHover.OnHoverExit();

                currentHover = newHover;

                if (currentHover != null) currentHover.OnHoverEnter();
            }
        }

        private void ClearHover()
        {
            if (currentHover != null)
            {
                currentHover.OnHoverExit();
                currentHover = null;
            }
        }
    }
}