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

        private HotBarItem slotItem = null;

        public override HotBarItem SlotItem { get => slotItem; set { slotItem = value; UpdateSlotUI(); } }

        public override void OnDrop(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public bool AddItem(HotBarItem itemToAdd)
        {
            if (SlotItem != null)
                return false;

            SlotItem = itemToAdd;
            return true;
        }

        public override void UpdateSlotUI()
        {
            throw new System.NotImplementedException();
        }
    }
}
