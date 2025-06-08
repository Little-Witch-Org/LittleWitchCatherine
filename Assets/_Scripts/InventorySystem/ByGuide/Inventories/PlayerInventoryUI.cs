using _Scripts.Managers;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.InventorySystem.ByGuide.Inventories
{
/// <summary>
/// Player inventory. UI links and representation. Info about items and other stats.
/// </summary>
    public class PlayerInventoryUI:MonoBehaviour, IMenu
    {
        //name of inventory etc
        //links to grids? info about items in grids etc ?
        
        
        [SerializeField] private GameObject contentParent;
        [SerializeField] Transform itemBufferContainerTransform;
        [SerializeField] private ItemGrid playerMainGrid;
        [SerializeField] private ItemGrid playerMagicGrid;
        [SerializeField] private ItemGrid playerPotionGrid;
        
        public GameObject ContentParent => contentParent;
        public void ShowMenu()
        {
            contentParent.SetActive(true);
            EventManager.Instance.InventoryEvents.InventoryOpenedStatusChanged(true, "PlayerInventory");
        }

        public void HideMenu()
        {
            contentParent.SetActive(false);
            EventManager.Instance.InventoryEvents.InventoryOpenedStatusChanged(false, "PlayerInventory");
        }

        public ItemGrid GetPlayerMainItemGrid()
        {
            return playerMainGrid;
        }

        public ItemGrid GetPlayerMagicItemGrid()
        {
            return playerMagicGrid;
        }
        
        public ItemGrid getPlayerPotionGrid()
        {
            return playerPotionGrid;
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