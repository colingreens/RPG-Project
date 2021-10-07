using UnityEngine;

namespace RPG.UI.Inventories
{
    [CreateAssetMenu(fileName = "New Rarity", menuName = "Items/Rarity")]
    public class Rarity : ScriptableObject
    {
        [SerializeField] private new string name = "New Rarity Name";
        [SerializeField] private Color textColor = new Color(1f, 1f, 1f, 1f);

        public string Name => name;
        public Color TextColor => textColor;
    }
}
