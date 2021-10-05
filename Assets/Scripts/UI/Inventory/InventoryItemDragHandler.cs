using UnityEngine.EventSystems;

namespace RPG.UI.Inventory
{
    public class InventoryItemDragHandler : ItemDragHandler
    {
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (eventData.hovered.Count == 0)
            {
                //destroy item or drop item
            }
        }
    }
}