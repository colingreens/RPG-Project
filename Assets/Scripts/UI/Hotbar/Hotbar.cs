using UnityEngine;

namespace RPG.UI.Hotbars
{
    public class Hotbar : MonoBehaviour
    {
        [SerializeField] private HotbarSlot[] hotbarSlots = new HotbarSlot[10];

        public void Add(HotBarItem itemToAdd)
        {
            foreach (var hotbarSlot in hotbarSlots)
            {
                if (hotbarSlot.AddItem(itemToAdd))
                    return;
            }
        }
    }
}
