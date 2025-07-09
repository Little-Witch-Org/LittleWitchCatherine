using System;

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
    }
}