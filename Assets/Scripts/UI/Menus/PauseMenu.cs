using Sirenix.OdinInspector;
using UnityEngine.Device;
using UnityEngine.SceneManagement;

namespace UI.Menus
{
    public class PauseMenuBase : MenuBase
    {
        [Button]
        private void DEBUG_OpenMenu()
        {
            OpenMenu();
        }
        
        public void LoadSave()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    
        public void ExitGame()
        {
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}