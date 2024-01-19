using System;
using UnityEngine;

namespace UI.Menus
{
    [Serializable]
    public class Page
    {
        [SerializeField] private GameObject display;

        public bool IsMatch(GameObject page)
        {
            return page.Equals(display);
        }
    
        public void ToggleDisplay(bool shouldDisplay)
        {
            display.SetActive(shouldDisplay);
        }
    }
}