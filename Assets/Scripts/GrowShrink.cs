using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrowShrink : MonoBehaviour
{
    private Movement playerMovement;
    // Define InputAction references
    private InputAction growAction;
    private InputAction shrinkAction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Collision with an enemy occurred
            Debug.Log("Player collided with Totem of Love");

            // Access the Movement script of the collided game object
            playerMovement = other.GetComponent<Movement>();

            // Get references to InputActions from the "Movement" action map
            growAction = playerMovement.actionMap.actions[1];
            shrinkAction = playerMovement.actionMap.actions[2];
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the script is found
        if (playerMovement != null)
        {
            // Check for key presses
            if (growAction.triggered)
            {
                // Increase radius
                Debug.Log("Player increased radius");
                playerMovement.ChangeRadius(1);
            }
            else if (shrinkAction.triggered)
            {
                // Decrease radius
                Debug.Log("Player decreased radius");
                playerMovement.ChangeRadius(-1);
            }
        }
    }
}
