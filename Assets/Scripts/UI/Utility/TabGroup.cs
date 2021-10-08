using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Utility
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private Sprite tabIdle;
        [SerializeField] private Sprite tabHover;
        [SerializeField] private Sprite tabActive;
        [SerializeField] private List<GameObject> objectToSwap = null;

        private List<TabButton> tabButtons = null;
        private TabButton selectedTab;

        public void Subscribe(TabButton button)
        {
            if (tabButtons == null)
            {
                tabButtons = new List<TabButton>();
            }

            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            if (selectedTab == null || button != selectedTab)
            {
                button.SetBackgroundImage(tabHover);
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            if (selectedTab != null)
            {
                selectedTab.Deselect();
            }

            selectedTab = button;
            selectedTab.Select();


            ResetTabs();
            button.SetBackgroundImage(tabActive);
            ActivateTabPage(button);
        }

        private void ActivateTabPage(TabButton button)
        {
            var index = button.transform.GetSiblingIndex();
            for (int i = 0; i < objectToSwap.Count; i++)
            {
                if (i == index)
                {
                    objectToSwap[i].SetActive(true);
                }
                else
                {
                    objectToSwap[i].SetActive(false);
                }                
            }
        }

        public void ResetTabs()
        {
            foreach (var tab in tabButtons)
            {
                if (tab == selectedTab)
                {
                    continue;
                }
                tab.SetBackgroundImage(tabIdle);
            }
        }
    }
}
