using RPG.UI.Inventories;
using UnityEngine;

namespace RPG.Combat
{   
    public class InventoryHandler : MonoBehaviour
    {
        [SerializeField] private Inventory inventory = null;

        public void AddToInventory(InventoryItem item)
        {
            inventory.AddItem(item);
    }
    }

    
}
