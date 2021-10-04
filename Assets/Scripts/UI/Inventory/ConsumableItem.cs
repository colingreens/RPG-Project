using System.Text;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class ConsumableItem : InventoryItem
    {
        [Header("Consumable Data")]
        [SerializeField] private string useText = "Does something, maybe?";


        public override string GetInfoDisplayText()
        {
            var builder = new StringBuilder();
            builder.Append(Name).AppendLine();
            builder.Append("<color=green>Use: ").Append(useText).Append("</color>").AppendLine();
            builder.Append("Max Stack: ").Append(MaxStack).AppendLine();
            builder.Append("Sell Price: ").Append(SellPrice).Append(" Gold");

            return builder.ToString();
        }
    }
}
