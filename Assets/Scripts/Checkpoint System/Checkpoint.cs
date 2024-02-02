using System;
using System.Collections.Generic;
using UnityEngine;

namespace Checkpoint_System
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private Vector3 pointToRespawn;
        [SerializeField] private float rotation;
        
        public static Action<Checkpoint> OnCheckpointReached;

        public Vector3 PointToRespawn => pointToRespawn;
        public float Rotation => rotation;
        
        private readonly List<Collider> playersWhoReachedZone = new(); 

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !playersWhoReachedZone.Contains(other))
            {
                playersWhoReachedZone.Add(other);
                if(playersWhoReachedZone.Count == 2)
                {
                    OnCheckpointReached?.Invoke(this);
                }
            }
        }
    }
}