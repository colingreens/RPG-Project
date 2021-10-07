namespace RPG.UI.Inventories
{
    public interface IItemContainer
    {
        ItemSlot AddItem(ItemSlot itemSlot);

        int GetTotalQuantity(InventoryItem item);

        bool HasItem(InventoryItem item);

        void RemoveAt(int slotIndex);

        void RemoveItem(ItemSlot itemSlot);

        void Swap(int indexOne, int indexTwo);
    }
}