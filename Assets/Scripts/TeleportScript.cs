using System;
using System.Collections;
using System.Collections.Generic;
using Checkpoint_System;
using UnityEngine;
public class TeleportScript : MonoBehaviour
{
    public GameObject toPoint;
    public GameObject[] players;
    public GameObject maze1;
    public GameObject maze2;
    public ZoneController bluePlayerZone;
    public ZoneController redPlayerZone;

    public bool pretendTeleport = false;

    private void FixedUpdate()
    {
        if (bluePlayerZone.isInZone && redPlayerZone.isInZone && !pretendTeleport)
        {
            foreach (GameObject player in players)
            {
                maze1.SetActive(true);
                maze2.SetActive(false);
                player.transform.position = toPoint.transform.position;
                bluePlayerZone.isInZone = false;
                redPlayerZone.isInZone = false;
            }
        }
        else if (bluePlayerZone.isInZone && redPlayerZone.isInZone && pretendTeleport)
        {
            maze1.SetActive(false);
            maze2.SetActive(true);
            bluePlayerZone.isInZone = false;
            redPlayerZone.isInZone = false;
        }
    }
}