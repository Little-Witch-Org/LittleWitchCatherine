using _Scripts.Enums;
using _Scripts.InventorySystem.ByGuide.Interfaces;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace _Scripts.InventorySystem.ByGuide.Scriptable
{
    
    [CreateAssetMenu(fileName = "ConsumableItemSo", menuName = "ScriptableObjects/Items/ConsumableItemSo")]
    public class ConsumableItemSo: ItemDataSo, IUsableItem
    {
        
        [Header("Consumable")] 
        [SerializeField] protected bool canUse;
        [SerializeField] protected bool isDeleteAfterUse;
        [SerializeField] protected bool isNewItemSpawnAfterUse;
        [SerializeField] protected ItemDataSo itemToSpawn;
        
        private const string HealthColumn = "item_health";
        private const string SatietyColumn = "item_satiety";
        private const string MoodColumn = "item_mood";
        private const string EnergyColumn = "item_energy";



        private int GetHealth()
        {
            string value = GetLocalizedCell(TableName, itemId, HealthColumn);
            return int.TryParse(value, out var result) ? result : 0;
        }

        private int GetSatiety()
        {
            string value = GetLocalizedCell(TableName, itemId, SatietyColumn);
            return int.TryParse(value, out var result) ? result : 0;
        }
        private int GetMood()
        {
            string value = GetLocalizedCell(TableName, itemId, MoodColumn);
            return int.TryParse(value, out var result) ? result : 0;
        }
        private int GetEnergy()
        {
            string value = GetLocalizedCell(TableName, itemId, EnergyColumn);
            return int.TryParse(value, out var result) ? result : 0;
        }

        public bool CanUseItem()
        {
            return canUse;
        }
        
        public bool IsDeleteAfterUse()
        {
            return isDeleteAfterUse;
        }

        public bool IsNewItemSpawnAfterUse()
        {
            return isNewItemSpawnAfterUse;
        }

        public ItemDataSo GerNewItemToSpawn()
        {
            return itemToSpawn;
        }

        public void UseItem()
        {
            if(!canUse){return;}
            
            if (GetHealth() != 0)
            {
                EventManager.Instance.PlayerStatsEvents.UpdateHealth(GetHealth());
            }

            if (GetSatiety() != 0)
            {
                EventManager.Instance.PlayerStatsEvents.UpdateSatiety(GetSatiety());
            }

            if (GetMood() != 0)
            {
                EventManager.Instance.PlayerStatsEvents.UpdateMood(GetMood());
            }

            if (GetEnergy() != 0)
            {
                EventManager.Instance.PlayerStatsEvents.UpdateEnergy(GetEnergy());
            }
            
        }
    }
}