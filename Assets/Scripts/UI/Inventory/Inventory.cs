using RPG.GameEvents.Events;
using UnityEngine;

namespace RPG.UI.Inventories
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Items/Inventory")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] private VoidEvent onInventoryItemsUpdated = null;

        [SerializeField] private ConsumableItem consumable = null;

        public ItemContainer ItemContainer { get; } = new ItemContainer(20);

        public void OnEnable()
        {
            ItemContainer.OnItemsUpdated += onInventoryItemsUpdated.Raise;
        }

        public void OnDisable()
        {
            ItemContainer.OnItemsUpdated -= onInventoryItemsUpdated.Raise;
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
    }
}
