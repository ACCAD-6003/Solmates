using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetism : MonoBehaviour
{
    [SerializeField]
    public float MaxDistanceToPull = 10;
    [SerializeField]
    public float SpeedToPull = 1;

    private float MinDistanceToPull = 0.1f;

    bool playerIsInteracting(Movement2 player)
    {
        return player.MovementDirection != 0 || player.RadiusDirection != 0;
    }

    bool playerIsClose(Movement2 player)
    {
        float distanceFrom = (player.transform.position - transform.position).magnitude;

        return distanceFrom <= MaxDistanceToPull && distanceFrom >= MinDistanceToPull;
    }

    // Update is called once per frame
    void Update()
    {
        Movement2[] players = FindObjectsOfType<Movement2>();

        foreach (Movement2 player in players)
        {
            if (!playerIsInteracting(player) && playerIsClose(player))
            {
                player.transform.position +=  Time.deltaTime * SpeedToPull * (transform.position - player.transform.position);
            }
        }
    }
}
