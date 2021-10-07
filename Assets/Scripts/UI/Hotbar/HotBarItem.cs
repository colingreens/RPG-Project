using UnityEngine;

namespace RPG.UI.Hotbars
{
    public abstract class HotBarItem : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private string name = "New Hotbar Item Name";
        [SerializeField] private Sprite icon = null;

        public string Name => name;
        public abstract string Coloredname { get; }

        public Sprite Icon => icon;

        public abstract string GetInfoDisplayText();

    }
}
