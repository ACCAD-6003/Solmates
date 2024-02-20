using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] bool requiresBothButtons = false;
    [SerializeField] bool doorStayOpen = false;
    [SerializeField] bool doorIsOpen = false;

    public ButtonController[] buttons;

    public void CheckIfDoorShouldOpen()
    {
        bool allButtonsPressed = true;
        foreach (ButtonController button in buttons)
        {
            if (button.isPressed)
            {
                if (!requiresBothButtons)
                {
                    doorIsOpen = true;
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (requiresBothButtons)
                {
                    allButtonsPressed = false;
                }
            }
        }

        if (allButtonsPressed && requiresBothButtons)
        {
            doorIsOpen = true;
            gameObject.SetActive(false);
        }
    }

    public void CheckIfDoorShouldClose()
    {
        foreach (ButtonController button in buttons)
        {
            if (button.isPressed)
            {
                doorIsOpen = true;
                gameObject.SetActive(false);
                break;
            }
            else
            {
                if (!doorStayOpen && doorIsOpen)
                {
                    doorIsOpen = false;
                    gameObject.SetActive(true);
                }
            }
        }
    }
}