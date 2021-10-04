using UnityEngine;

namespace RPG.UI.Inventory
{
    public class Inventory : ScriptableObject, IItemContainer
    {
        public ItemSlot AddItem(ItemSlot itemSlot)
        {
            return default;
        }

        public int GetTotalQuantity(InventoryItem item)
        {
            return 0;
        }

        public bool HasItem(InventoryItem item)
        {
            return false;
        }

        public void RemoveAt(int slotIndex)
        {
        }

        public void RemoveItem(ItemSlot itemSlot)
        {
        }

        public void Swap(int indexOne, int indexTwo)
        {
        }
    }
}
