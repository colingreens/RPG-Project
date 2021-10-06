using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI.Inventory
{
    public abstract class ItemSlotUI : MonoBehaviour, IDropHandler
    {
        public abstract HotBarItem SlotItem { get; set; }

        public int SlotIndex { get; private set; }

        public abstract void OnDrop(PointerEventData eventData);

        public abstract void UpdateSlotUI();

        protected virtual void EnableSlotUI(bool enable)
        {
            itemIconImage.enabled = enable;
        }

        protected virtual void Start()
        {
            SlotIndex = transform.GetSiblingIndex();
            UpdateSlotUI();
        }

        private void OnEnable() => UpdateSlotUI();

        [SerializeField]
        protected Image itemIconImage = null;
    }
}