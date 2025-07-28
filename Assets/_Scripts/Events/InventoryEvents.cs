using System;
using _Scripts.InventorySystem.ByGuide;

namespace _Scripts.Events
{
    public class InventoryEvents
    {
        public event Action<bool> OnSelectedItemStatusChanged;
        public void SelectedItemStatusChanged(bool isSelected)
        {
            OnSelectedItemStatusChanged?.Invoke(isSelected);
        }
        
        
        public event Action<bool,string> OnInventoryOpenedStatusChanged;
        public void InventoryOpenedStatusChanged(bool isOpened, string inventoryName)
        {
            OnInventoryOpenedStatusChanged?.Invoke(isOpened, inventoryName);
        }
        
        
        public event Action<bool> OnContextMenuOpenStatusSet;
        public void SetContextMenuOpenStatus(bool isOpened)
        {
            OnContextMenuOpenStatusSet?.Invoke(isOpened);
        }
        
        public event Action<string,string> OnAddItem;
        public void AddItem(string itemNameOrID, string itemGridName)
        {
            OnAddItem?.Invoke(itemNameOrID, itemGridName);
        }
        public event Action<InventoryItem> OnItemUsed;
        public void ItemUsed(InventoryItem item)
        {
            OnItemUsed?.Invoke(item);
        }
        
    }
}