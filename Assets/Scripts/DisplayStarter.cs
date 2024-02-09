using UnityEngine;

public class DisplayStarter : SingletonMonoBehavior<DisplayStarter>
{
    private bool secondDisplayFound;
    
    private void Start()
    {
        Display.onDisplaysUpdated += ToggleDisplay;
        Display.displays[0].Activate();
    }

    private void ToggleDisplay()
    {
        Debug.Log("Screem");
    }
}