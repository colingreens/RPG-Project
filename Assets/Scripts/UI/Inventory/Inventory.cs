using System;
using System.Linq;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class Inventory : ScriptableObject, IItemContainer
    {
        public Action OnItemsUpdated = delegate { }; 

        private ItemSlot[] itemSlots = new ItemSlot[20];

        public ItemSlot GetSlotByIndex(int index) => itemSlots[index];

        public ItemSlot AddItem(ItemSlot itemSlot)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item == null)
                    continue;
                if (itemSlots[i].item != itemSlot.item)
                    continue;

                var slotRemainingSpace = itemSlots[i].item.MaxStack - itemSlots[i].quantity;

                if (itemSlot.quantity <= slotRemainingSpace)
                {
                    itemSlots[i].quantity += itemSlot.quantity;
                    itemSlot.quantity = 0;

                    OnItemsUpdated.Invoke();
                    return itemSlot;
                }

                if (slotRemainingSpace > 0)
                {
                    itemSlots[i].quantity += slotRemainingSpace;
                    itemSlot.quantity -= slotRemainingSpace;
                }
            }

            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item != null)
                    continue;

                if (itemSlot.quantity <= itemSlot.item.MaxStack)
                {
                    itemSlots[i] = itemSlot;
                    itemSlot.quantity = 0;

                    OnItemsUpdated.Invoke();
                    return itemSlot;
                }

                itemSlots[i] = new ItemSlot(itemSlot.item, itemSlot.item.MaxStack);
                itemSlot.quantity -= itemSlot.item.MaxStack;
            }

            OnItemsUpdated.Invoke();
            return itemSlot;
        }

        public int GetTotalQuantity(InventoryItem item)
        {
            var totalCount = 0;

            foreach (var itemSlot in itemSlots)
            {
                if (itemSlot.item == null)
                    continue;

                if (itemSlot.item != item)
                    continue;

                totalCount += itemSlot.quantity;
            }

            return totalCount;
        }

        public bool HasItem(InventoryItem item)
        {
            return itemSlots.Select(x => x.item).Contains(item);
        }

        public void RemoveAt(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > itemSlots.Length - 1)
                return;

            itemSlots[slotIndex] = new ItemSlot();

            OnItemsUpdated.Invoke();

        }

        public void RemoveItem(ItemSlot itemSlot)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item != null)
                    continue;
                if (itemSlots[i].item != itemSlot.item)
                    continue;

                if (itemSlots[i].quantity < itemSlot.quantity)
                {
                    itemSlot.quantity -= itemSlots[i].quantity;
                    itemSlots[i] = new ItemSlot();
                }
                else
                {
                    itemSlots[i].quantity -= itemSlot.quantity;
                    if (itemSlots[i].quantity == 0)
                    {
                        itemSlots[i] = new ItemSlot();

                        OnItemsUpdated.Invoke();

                        return;
                    }
                }
            }
        }

        public void Swap(int indexOne, int indexTwo)
        {
            var firstSlot = itemSlots[indexOne];
            var secondSlot = itemSlots[indexTwo];

            if (firstSlot == secondSlot)
                return;

            if (secondSlot.item != null)
            {
                if (firstSlot.item == secondSlot.item)
                {
                    var secondSlotRemainingSpace = secondSlot.item.MaxStack - secondSlot.quantity;

                    if (firstSlot.quantity <= secondSlotRemainingSpace)
                    {
                        secondSlot.quantity += firstSlot.quantity;

                        itemSlots[indexOne] = new ItemSlot();
                        OnItemsUpdated.Invoke();
                        return;
                    }
                }
            }

            itemSlots[indexOne] = secondSlot;
            itemSlots[indexTwo] = firstSlot;
            OnItemsUpdated.Invoke();
        }
    }
}
