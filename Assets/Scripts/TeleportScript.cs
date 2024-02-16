using System;
using System.Collections;
using System.Collections.Generic;
using Checkpoint_System;
using UnityEngine;
public class TeleportScript : MonoBehaviour
{
    public GameObject toPointNormal;
    public GameObject toPointPlayerNotInTPZone;
    public GameObject[] players;
    public GameObject maze1;
    public GameObject maze2;

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
            maze1.SetActive(false);
            maze2.SetActive(true);
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
                maze1.SetActive(true);
                maze2.SetActive(false);
                player.transform.position = toPointNormal.transform.position;
            }            
        }
    }
}