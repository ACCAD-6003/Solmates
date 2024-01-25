using System;
using Controller;
using Sirenix.OdinInspector;
using UnityEngine.Device;
using UnityEngine.SceneManagement;

namespace UI.Menus
{
    public class PauseMenu : MenuBase
    {
        protected override Action OpenTrigger { get => UIController.OnPause; set => UIController.OnPause = value; }
        protected override Action CloseTrigger { get => UIController.OnGoBack; set => UIController.OnGoBack = value; }
        
        [Button]
        private void DEBUG_OpenMenu()
        {
            OpenMenu();
        }
        
        [Button]
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    
        [Button]
        public void ExitGame()
        {
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}