using UnityEngine;

namespace RPG.UI.Inventory
{
    public abstract class InventoryItem : HotBarItem
    {
        [Header("Item Data")]
        [SerializeField] [Min(0)] private int sellPrice = 1;
        [SerializeField] [Min(1)] private int maxStack = 1;

        public override string Coloredname
        {
            get
            {
                return Name;
            }
        }

        public int SellPrice => sellPrice;

        public int MaxStack => maxStack;
    }
}
