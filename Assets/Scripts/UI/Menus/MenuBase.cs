using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Menus
{
    public abstract class MenuBase : MonoBehaviour
    {
        [SerializeField] private GameObject display;
        [SerializeField] private List<Page> pages;

        private int currentPage;

        private void OnEnable()
        {
            CloseMenu();
        }

        public virtual void OpenMenu()
        {
            display.SetActive(true);
            JumpToPage(0);
        }

        public void JumpToPage(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= pages.Count)
            {
                Debug.LogError("Invalid page index");
                return;
            }

            pages[currentPage].ToggleDisplay(false);
            pages[pageIndex].ToggleDisplay(true);
            currentPage = pageIndex;
        }
    
        public void JumpToPage(GameObject page)
        {
            JumpToPage(pages.FindIndex(p => p.IsMatch(page)));
        }

        private void HideAllPages()
        {
            pages.ForEach(p => p.ToggleDisplay(false));
        }

        public virtual void CloseMenu()
        {
            display.SetActive(false);
            HideAllPages();
        }
    }
}