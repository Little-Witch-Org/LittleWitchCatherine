using System;
using System.Linq;
using _Scripts.InventorySystem.ByGuide;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.Service
{
    public class MenuDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject contentParent;
        [SerializeField] private RectTransform window; // Окно, которое будем двигать
        [SerializeField] private Image dragHandle; 
        private Vector2 _dragOffset; // Смещение курсора при захвате
        
        private bool _canDrag;

        private bool _isInventoryItemPickedUp;

        private void OnEnable()
        {
            EventManager.Instance.InventoryEvents.OnSelectedItemStatusChanged += SetItemPickedUpStatusChanged;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.InventoryEvents.OnSelectedItemStatusChanged -= SetItemPickedUpStatusChanged;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return; 
            
            // 1. Клик именно на _dragHandle
            // 2. На целевом окне есть WindowDrag
            _canDrag = eventData.pointerCurrentRaycast.gameObject == dragHandle.gameObject 
                       && window.GetComponent<MenuDragHandler>() != null;

            if (!_canDrag) return;
            if (_isInventoryItemPickedUp)
            {
                return;
            }
            
            
            
            InventoryManager.Instance.SetWindowDraggingState(true, contentParent.transform.parent.gameObject);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                window,
                eventData.position,
                eventData.pressEventCamera,
                out _dragOffset
            );
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return; 
            
            if (!_canDrag) return;
            if (_isInventoryItemPickedUp)
            {
                return;
            }
            
            // Перемещаем окно с учетом смещения
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    window.parent as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 cursorPos
                ))
            {
                window.localPosition = cursorPos - _dragOffset;
                ClampToScreenBounds(); // Ограничиваем границы
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            InventoryManager.Instance.SetWindowDraggingState(false, contentParent.transform.parent.gameObject); //todo use events ?
        }

        // Ограничение, чтобы окно не выходило за экран
        private void ClampToScreenBounds()
        {
            Vector3[] corners = new Vector3[4];
            window.GetWorldCorners(corners);

            float minX = corners.Min(c => c.x);
            float maxX = corners.Max(c => c.x);
            float minY = corners.Min(c => c.y);
            float maxY = corners.Max(c => c.y);

            Vector3 correction = Vector3.zero;

            if (maxX > Screen.width) correction.x = Screen.width - maxX;
            if (minX < 0) correction.x = -minX;
            if (maxY > Screen.height) correction.y = Screen.height - maxY;
            if (minY < 0) correction.y = -minY;

            window.position += correction;
        }

        private void SetItemPickedUpStatusChanged(bool isInventoryItemPickedUp)
        {
            //Debug.Log("change state to "+ isInventoryItemPickedUp);
            _isInventoryItemPickedUp = isInventoryItemPickedUp;
        }
    }
}