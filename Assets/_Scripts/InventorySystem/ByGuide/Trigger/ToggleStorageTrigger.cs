using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.InventorySystem.ByGuide.Trigger
{
    public class ToggleStorageTrigger : MonoBehaviour
    {
        public void ToggleStorage()
        {
            EventManager.Instance.InputEvents.StorageInventoryPressed();
        }
        
        public void ToggleStorageUnderStairs()
        {
            EventManager.Instance.InputEvents.StorageUnderStairsInventoryPressed();
        }
    }
}