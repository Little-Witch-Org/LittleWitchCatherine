using System;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _Scripts.InventorySystem.ByGuide
{
    [RequireComponent(typeof(ItemGrid))]
    public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private InventoryManager _inventoryManager;
        private ItemGrid _itemGrid;
        
        
        private void Start()
        {
            _inventoryManager = InventoryManager.Instance;
            _itemGrid = GetComponent<ItemGrid>();
        }

        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(_inventoryManager.GetIsInventoryWindowDragging()){return;}
            _inventoryManager.SelectedItemGrid=_itemGrid;// set current item grid to controller (manager) on hover
        }

        public void OnPointerExit(PointerEventData eventData) 
        {
            _inventoryManager.SelectedItemGrid=null;//remove link
        }
    }
}