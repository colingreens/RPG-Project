using RPG.UI.Inventories;
using UnityEngine;

namespace RPG.Combat
{   
    public class InventoryHandler : MonoBehaviour
    {
        [SerializeField] private Inventory inventory = null;
        [SerializeField] private WeaponConfig weapon = null;

        private Fighter fighter;
        
        private void Awake()
        {
            fighter = GetComponent<Fighter>();
        }

        public void AddToInventory(InventoryItem item)
        {
            inventory.AddItem(item);
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            fighter.EquipWeapon(weapon);
        }

        [ContextMenu("Test Add Weapon")]
        public void EquipWeapon()
        {
            fighter.EquipWeapon(weapon);
        }
    }

    
}
