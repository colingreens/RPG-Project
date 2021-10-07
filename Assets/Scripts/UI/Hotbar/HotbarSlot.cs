using TMPro;
using RPG.UI.Inventories;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Hotbars
{
    public class HotbarSlot : ItemSlotUI, IDropHandler
    {
        [SerializeField] private Inventory inventory = null;
        [SerializeField] private TextMeshProUGUI itemQuantityText = null;
        //set cooldown for abilities

        private HotBarItem slotItem = null;

        public override HotBarItem SlotItem { get => slotItem; set { slotItem = value; UpdateSlotUI(); } }

        public override void OnDrop(PointerEventData eventData)
        {
            var itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
            if (itemDragHandler == null)
                return;

            var inventorySlot = itemDragHandler.ItemSlotUI as InventorySlot;
            if (inventorySlot != null)
            {
                SlotItem = inventorySlot.ItemSlot.item;
                return;
            }

            var hotbarSlot = itemDragHandler.ItemSlotUI as HotbarSlot;
            if (hotbarSlot != null)
            {
                var oldItem = SlotItem;
                SlotItem = hotbarSlot.SlotItem;
                hotbarSlot.SlotItem = oldItem;
                return;
            }
        }

        public bool AddItem(HotBarItem itemToAdd)
        {
            if (SlotItem != null)
                return false;

            SlotItem = itemToAdd;
            return true;
        }

        public void UseSlot(int index)
        {
            if (index != SlotIndex)
                return;

            //use item
        }

        public override void UpdateSlotUI()
        {
            if (SlotItem == null)
            {
                EnableSlotUI(false);
                return;
            }

            itemIconImage.sprite = SlotItem.Icon;
            EnableSlotUI(true);

            SetItemQuantityUI();
            //update cooldown
        }

        protected override void EnableSlotUI(bool enable)
        {
            base.EnableSlotUI(enable);
            itemQuantityText.enabled = enable;
        }

        private void SetItemQuantityUI()
        {
            if (SlotItem is InventoryItem inventoryItem)
            {
                if (inventory.ItemContainer.HasItem(inventoryItem))
                {
                    var quantityCount = inventory.ItemContainer.GetTotalQuantity(inventoryItem);
                    itemQuantityText.text = quantityCount > 1 ? quantityCount.ToString() : "";
                }
                else
                {
                    SlotItem = null;
                }
            }
            else 
            {
                itemQuantityText.enabled = false;
            }
        }
    }
}
