using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.GameEvents.Events;

namespace RPG.UI.Utility
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler,IPointerExitHandler
    {
        [SerializeField] private TabGroup tabGroup = null;
        [SerializeField] private Image backgroundImage = null;
        [SerializeField] private VoidEvent onSelect = null;
        [SerializeField] private VoidEvent onDeselect = null;

        private void Awake()
        {
            backgroundImage = GetComponent<Image>();            
        }

        private void Start()
        {
            tabGroup.Subscribe(this);
        }

        public void SetBackgroundImage(Sprite image)
        {
            backgroundImage.sprite = image;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void Select()
        {
            if (onSelect != null)
            {
                onSelect.Raise();
            }
            
        }

        public void Deselect()
        {
            if (onDeselect != null)
            {
                onDeselect.Raise();
            }            
        }
    }
}
