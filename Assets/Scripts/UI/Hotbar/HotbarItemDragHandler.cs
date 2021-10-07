using RPG.UI.Inventories;
using UnityEngine.EventSystems;

namespace  RPG.UI.Hotbars
{
    public class HotbarItemDragHandler : ItemDragHandler
    {
        public override void OnPointerUp(PointerEventData eventData)
        {            
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                base.OnPointerUp(eventData);

                if (eventData.hovered.Count == 0)
                {
                    (ItemSlotUI as HotbarSlot).SlotItem = null;
                }
            }
        }
    }
}
