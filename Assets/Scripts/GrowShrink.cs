using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrowShrink : MonoBehaviour
{
    public Movement playerMovement;
    // Define InputAction references
    public InputAction growAction;
    public InputAction shrinkAction;
    public GameObject[] players;

    // Start is called before the first frame update
    private void Start(){
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Collision with an enemy occurred
            Debug.Log("Player collided with Totem of Love");

            // Access the Movement script of the collided game object
            playerMovement = other.GetComponent<Movement>();

            // Get references to InputActions from the "Movement" action map
            // playerMovement.actionMap.actionTriggered += playerMovement.TotemActionTriggered;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Collision with an enemy occurred
            Debug.Log("Player left Totem of Love");

            // Get references to InputActions from the "Movement" action map
            // playerMovement.actionMap.actionTriggered -= playerMovement.TotemActionTriggered;
        }
    }

    private void Update()
    {
        // Check if the script is found
        if (playerMovement != null)
        {
            // // Check for key presses
            // if (playerMovement.totemAction == "Grow" || playerMovement.totemAction == "Shrink")
            // {
            //     Debug.Log("Action triggered");
            //     for (int i = 0; i < players.Length; i++) {
            //         Movement tempMovement = players[i].GetComponent<Movement>();
            //         if (playerMovement.totemAction == "Grow") {
            //             // Increase radius
            //             Debug.Log("Player increased radius");
            //             tempMovement.ChangeRadius(Time.deltaTime);
            //         }
            //         else if (playerMovement.totemAction == "Shrink") {
            //             // Decrease radius
            //             Debug.Log("Player decreased radius");
            //             tempMovement.ChangeRadius(-Time.deltaTime);
            //         }
            //     }
            // }
        }
    }
}
