using RPG.UI.Hotbars;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public abstract class InventoryItem : HotBarItem
    {
        [Header("Item Data")]
        [SerializeField] private Rarity rarity = null;
        [SerializeField] [Min(0)] private int sellPrice = 1;
        [SerializeField] [Min(1)] private int maxStack = 1;

        public override string Coloredname
        {
            get
            {
                var hexColor = ColorUtility.ToHtmlStringRGB(rarity.TextColor);
                return $"<color=#{hexColor}>{Name}</color>";
            }
        }

        public int SellPrice => sellPrice;

        public int MaxStack => maxStack;

        public Rarity Rarity => rarity;
    }
}
