using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool isPressed = false;
    [SerializeField] DoorController door;
    [SerializeField] GameObject pressedButton;
    [SerializeField] GameObject unpressedButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = true;
            door.CheckIfDoorShouldOpen();
            pressedButton.SetActive(true);
            unpressedButton.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = false;
            door.CheckIfDoorShouldClose();
            pressedButton.SetActive(false);
            unpressedButton.SetActive(true);
        }
    }
}