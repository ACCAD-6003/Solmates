using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public GameObject toPoint;
    public GameObject players;

    public int numOfPlayersInZone = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            numOfPlayersInZone++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            numOfPlayersInZone--;
        }
    }

    void FixedUpdate()
    {
        if (numOfPlayersInZone == 2)
        {
            players.transform.position = toPoint.transform.position;
        }
    }
}