using RPG.UI.Hotbars;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Inventories
{
    public class InventorySlot : ItemSlotUI, IDropHandler
    {
        [SerializeField] private Inventory inventory = null;
        [SerializeField] private TextMeshProUGUI itemQualityText = null;

        public override HotBarItem SlotItem
        {
            get => ItemSlot.item;
            set { }
        }

        public ItemSlot ItemSlot => inventory.ItemContainer.GetSlotByIndex(SlotIndex);

        public override void OnDrop(PointerEventData eventData)
        {
            var itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();

            if (itemDragHandler == null)
                return;

            if ((itemDragHandler.ItemSlotUI as InventorySlot) != null)
            {
                inventory.ItemContainer.Swap(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
            }
        }

        public override void UpdateSlotUI()
        {
            if (ItemSlot.item == null)
            {
                EnableSlotUI(false);
                return;
            }

            EnableSlotUI(true);
            itemIconImage.sprite = ItemSlot.item.Icon;
            itemQualityText.text = ItemSlot.quantity > 1 ? ItemSlot.quantity.ToString() : "";
        }

        protected override void EnableSlotUI(bool enable)
        {
            base.EnableSlotUI(enable);
            itemQualityText.enabled = enable;
        }

        
    }
}