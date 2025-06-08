using _Scripts.Managers;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.InventorySystem.ByGuide.Inventories
{
    public class StorageInventoryUI : MonoBehaviour, IMenu
    {
        
        [SerializeField] private GameObject contentParent;
        [SerializeField] Transform itemBufferContainerTransform;
        [SerializeField] private ItemGrid storageGrid;
        [SerializeField] private GameObject storageIcon;
        

        public GameObject ContentParent => contentParent;
        public void ShowMenu()
        {
            contentParent.SetActive(true);
            EventManager.Instance.InventoryEvents.InventoryOpenedStatusChanged(true, "StorageInventory");
        }

        public void HideMenu()
        {
            contentParent.SetActive(false);
            EventManager.Instance.InventoryEvents.InventoryOpenedStatusChanged(false, "StorageInventory");
        }
        
        public ItemGrid GetStorageItemGrid()
        {
            return storageGrid;
        }

        public Transform GetItemBufferContainerTransform()
        {
            return itemBufferContainerTransform;
        }

        public GameObject GetContentParent()
        {
            return contentParent;
        }
        
    }
}