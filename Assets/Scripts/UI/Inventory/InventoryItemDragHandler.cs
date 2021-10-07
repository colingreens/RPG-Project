using UnityEngine.EventSystems;
using UnityEngine;
using RPG.Control;

namespace RPG.UI.Inventories
{
    public class InventoryItemDragHandler : ItemDragHandler
    {
        [SerializeField] private ItemDestroyer itemDestroyer = null;
        [SerializeField] private PlayerController playerController = null;

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            playerController.enabled = true;

            if (eventData.hovered.Count == 0)
            {                
                var thisSlot = ItemSlotUI as InventorySlot;
                itemDestroyer.Activate(thisSlot.ItemSlot, thisSlot.SlotIndex);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            playerController.enabled = false;
        }
    }
}