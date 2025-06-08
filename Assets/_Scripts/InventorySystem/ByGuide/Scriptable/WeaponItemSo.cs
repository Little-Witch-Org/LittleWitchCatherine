using UnityEngine;
using UnityEngine.Localization;

namespace _Scripts.InventorySystem.ByGuide.Scriptable
{
    [CreateAssetMenu(fileName = "ConsumableItem", menuName = "ScriptableObjects/Items/ConsumableItemSo")]
    public class WeaponItemSo:ItemDataSo
    {
        public LocalizedString localizedDamage;
        public LocalizedString localizedAttackSpeed;

        public int Damage => int.Parse(localizedDamage.GetLocalizedString());
        public float AttackSpeed => float.Parse(localizedAttackSpeed.GetLocalizedString());
    }
}