using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.InventorySystem.ByGuide.Interfaces;
using _Scripts.InventorySystem.ByGuide.Scriptable;
using _Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.InventorySystem.ByGuide
{
    public class ContextMenuController : MonoBehaviour
    {
        //public static TooltipController Instance; //there is one tooltip object on scene.

        [SerializeField] private RectTransform backgroundTransform; //need to add offset
        
        [SerializeField] private Button useItemButton;
        [SerializeField] private Button deleteItemButton;
        [SerializeField] private TMP_Text useItemText;
        [SerializeField] private TMP_Text deleteItemText;
        
        [SerializeField] private Vector2 offset;

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        
        private InventoryItem _currentItem;
        private string _parentGridName; //contains grid name to which items belongs

        void Awake()
        {
            //Instance = this;
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            HideContextMenu();
            
        }

        private void OnEnable()
        {
            EventManager.Instance.InventoryEvents.OnInventoryOpenedStatusChanged += OnInventoryStatusChanged;
        }

        private void OnDisable()
        {
            EventManager.Instance.InventoryEvents.OnInventoryOpenedStatusChanged -= OnInventoryStatusChanged;
        }

        private void Update()
        {
            if (_canvasGroup.alpha == 0) return;

            //close menu if click out of menu bounds
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsMouseOverRectTransform(backgroundTransform))
                {
                    HideContextMenu();
                }
            }
        }


        private string ExtractParentGridName(string gridName)
        {
            return gridName.Split('(')[0].Trim();
        }

        public void ShowContextMenu(InventoryItem item, string currentItemGridName)
        {
            _parentGridName = ExtractParentGridName(currentItemGridName);
                
            ConfigureMenuBeforeDisplay(item);
            
            EventManager.Instance.InventoryEvents.SetContextMenuOpenStatus(true);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundTransform);
            

            Vector2 contextMenu = backgroundTransform.rect.size;
            float cMyOffset = contextMenu.x * 0.5f;

            //todo to check borders use method from tooltip
            _rectTransform.position = new Vector2(
                Input.mousePosition.x + offset.x + cMyOffset,
                Input.mousePosition.y + offset.y);
            
            
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }


        private void HideContextMenu()
        {
            EventManager.Instance.InventoryEvents.SetContextMenuOpenStatus(false);
            
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _parentGridName = "";
        }

        private void ConfigureMenuBeforeDisplay(InventoryItem item)
        {
            _currentItem = item;
            
            //Debug.Log(item);
            //Debug.Log(item.itemData.itemId);
            //Debug.Log(item.itemData is IUsableItem);
            
            //----add localized text to buttons
            
            //clear button listeners
            useItemButton.onClick.RemoveAllListeners();
            deleteItemButton.onClick.RemoveAllListeners();
            
            //use button
            if (item.itemData is IUsableItem usableItem)
            {
                if (usableItem.CanUseItem())
                {
                    useItemButton.gameObject.SetActive(true);
                    useItemButton.onClick.AddListener(OnUseItem);
                }
                else
                {
                    useItemButton.gameObject.SetActive(false);
                }
            }
            else
            {
                useItemButton.gameObject.SetActive(false);
            }
            
            //delete button
            deleteItemButton.onClick.AddListener(OnDeleteItem);
            
            
            //лаконичная версия
            // Use button
            /*var usableItem = item.itemData as IUsableItem;
            bool canUse = usableItem?.CanUseItem() == true;

            useItemButton.gameObject.SetActive(canUse);
            useItemButton.onClick.RemoveAllListeners();
            if (canUse)
                useItemButton.onClick.AddListener(OnUseItem);*/
            
        }


        private void OnUseItem()
        {
            if (_currentItem.itemData is IUsableItem usable)
            {
                usable.UseItem();

                if (_currentItem.itemData is ConsumableItemSo consumableItemSo)
                {
                    if (consumableItemSo.IsDeleteAfterUse())
                    {
                        InventoryManager.Instance.RemoveItem(_currentItem);
                        if (consumableItemSo.IsNewItemSpawnAfterUse())
                        {
                            var newItem = consumableItemSo.GerNewItemToSpawn();
                            if (newItem != null)
                            {
                                StartCoroutine(SpawnNewItemNextFrame(newItem));
                            }
                            else
                            {
                                Debug.LogWarning($"Item {_currentItem.itemData.itemId} has no spawnable item");
                            }
                        }
                    }

                    EventManager.Instance.InventoryEvents.ItemUsed(_currentItem);
                }
            }

            HideContextMenu();
        }
        
        private IEnumerator SpawnNewItemNextFrame(ItemDataSo newItem)
        {
            yield return null; // 1 frame to spawn in the same place (in no other slots) //todo spawn in same slot ?? need to get position
            Debug.Log(_currentItem.itemData.itemId);
            Debug.Log(_parentGridName);
            EventManager.Instance.InventoryEvents.AddItem(newItem.itemId, _parentGridName); 
        }

        private void OnDeleteItem()
        {
            
            InventoryManager.Instance.RemoveItem(_currentItem);
            HideContextMenu();
        }

        private bool IsMouseOverRectTransform(RectTransform rectTransform)
        {
            Vector2 localMousePosition;
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(
                       rectTransform, Input.mousePosition, null, out localMousePosition)
                   && rectTransform.rect.Contains(localMousePosition);
        }

        private void OnInventoryStatusChanged(bool isOpen, string inventoryName)
        {
            //handle close inventory (only need for player with hotkey)
            if (inventoryName.Contains("PlayerInventory") && isOpen == false)
            {
                HideContextMenu();
            }
        }
    }
}