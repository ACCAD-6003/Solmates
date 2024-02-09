using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayStarter : SingletonMonoBehavior<DisplayStarter>
{
    [SerializeField] private bool debugMode;
    
    private static bool SecondDisplayFound => Display.displays.Length > 1;

    private void Start()
    {
        Display.onDisplaysUpdated += ToggleDisplay;
        SceneManager.sceneLoaded += (_, _) => ToggleDisplay();
        ToggleDisplay();
    }

    [Button("DEBUG Refresh Display")]
    private void ToggleDisplay()
    {
        var blueCamera = GameObject.FindGameObjectWithTag("BlueCamera").GetComponent<Camera>();
        var redCamera = GameObject.FindGameObjectWithTag("RedCamera").GetComponent<Camera>();

        blueCamera.targetDisplay = 0;
        Debug.Log(SecondDisplayFound);
        
        if (debugMode || SecondDisplayFound)
        {
            if (SecondDisplayFound && !Display.displays[1].active) Display.displays[1].Activate();
            blueCamera.targetDisplay = 1;
            blueCamera.rect = new Rect(0f, 0, 1, 1);
            redCamera.rect = new Rect(0, 0, 1, 1);
        }
        else
        {
            blueCamera.targetDisplay = 0;
            blueCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
            redCamera.rect = new Rect(0, 0, 0.5f, 1);
        }
    }
}