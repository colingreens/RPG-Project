using RPG.UI.Inventory;
using UnityEngine;

namespace RPG.GameEvents.Events
{
    [CreateAssetMenu(fileName = "New Hotbar Item Event", menuName = "Game Events/Hotbar Item" )]
    public class HotbarItemEvent : BaseGameEvent<HotBarItem>
    {
    }
}
