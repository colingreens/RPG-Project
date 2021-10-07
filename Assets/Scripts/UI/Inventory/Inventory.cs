using RPG.Combat;
using RPG.GameEvents.Events;
using UnityEngine;

namespace RPG.UI.Inventories
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Items/Inventory")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] private VoidEvent onInventoryItemsUpdated = null;

        [SerializeField] private ConsumableItem consumable = null;

        [SerializeField] private WeaponConfig weapon = null;

        public ItemContainer ItemContainer { get; } = new ItemContainer(20);

        public void OnEnable()
        {
            ItemContainer.OnItemsUpdated += onInventoryItemsUpdated.Raise;
        }

        public void OnDisable()
        {
            ItemContainer.OnItemsUpdated -= onInventoryItemsUpdated.Raise;
        }

        public void AddItem(InventoryItem item)
        {
            ItemContainer.AddItem(new ItemSlot
            {
                item = item,
                quantity = 1
            });
        }

        [ContextMenu("Test Add")]
        public void TestAdd()
        {
            ItemContainer.AddItem(new ItemSlot
            {
                item = consumable,
                quantity = 5
            });
        }

        [ContextMenu("Test Add Weapon")]
        public void TestAddWeapon()
        {
            ItemContainer.AddItem(new ItemSlot
            {
                item = weapon,
                quantity = 1
            });
        }
    }
}
