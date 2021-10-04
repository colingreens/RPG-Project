using UnityEngine;

namespace RPG.UI.Inventory
{
    public struct ItemSlot
    {
        public ItemSlot(InventoryItem item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

        public InventoryItem item;
        [Min(1)] public int quantity;

        public static bool operator ==(ItemSlot a, ItemSlot b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(ItemSlot a, ItemSlot b)
        {
            return !a.Equals(b);
        }
    }
}