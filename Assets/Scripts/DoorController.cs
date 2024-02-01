using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool shouldStayOpen = false;
    public bool isOpen = false;
    public GameObject[] buttons;

    private bool allButtonsPressed = false;

    private void Update()
    {
        // Check if all buttons are pressed
        allButtonsPressed = true;
        foreach (GameObject button in buttons)
        {
            if (!button.GetComponent<ButtonController>().isPressed)
            {
                allButtonsPressed = false;
                break;
            }
        }

        // Deactivate door if all buttons are pressed
        if (allButtonsPressed)
        {
            isOpen = false;
            gameObject.SetActive(false);
        }
        // Reactivate door if any button is released
        else if (!isOpen && gameObject.activeSelf == false)
        {
            if (shouldStayOpen)
            {
                isOpen = true;
                gameObject.SetActive(true);
            }
            // Add other conditions for closing the door if needed
        }
    }
}
