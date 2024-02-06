using System;
using System.Collections;
using System.Collections.Generic;
using Checkpoint_System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TeleportScript : MonoBehaviour
{
    public GameObject toPointNormal;
    public GameObject toPointPlayerNotInTPZone;
    private GameObject playerInTPZone;
    public GameObject[] players;

    public bool moreThanOneTeleportPoint = false;
    private int numOfPlayersInZone = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !moreThanOneTeleportPoint)
        {
            numOfPlayersInZone++;
        }
        else if (moreThanOneTeleportPoint)
        {
            playerInTPZone = other.gameObject;
            playerInTPZone.transform.position = toPointNormal.transform.position;

            foreach (GameObject player in players)
            {
                if (player != playerInTPZone)
                {
                    player.transform.position = toPointPlayerNotInTPZone.transform.position;
                    break;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !moreThanOneTeleportPoint)
        {
            numOfPlayersInZone--;
        }
    }

    void FixedUpdate()
    {
        if (numOfPlayersInZone == 2 && !moreThanOneTeleportPoint)
        {
            foreach (GameObject player in players)
            {
                player.transform.position = toPointNormal.transform.position;
            }            
        }
    }
}