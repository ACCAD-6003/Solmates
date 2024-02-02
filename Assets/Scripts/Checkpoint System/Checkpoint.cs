using System;
using System.Collections.Generic;
using UI.Dialogue_System;
using UnityEngine;

namespace Checkpoint_System
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private Vector3 pointToRespawn;
        [SerializeField] private float rotation;
        [SerializeField] private SOConversationData conversation;
        
        public static Action<Checkpoint> OnCheckpointReached;

        public Vector3 PointToRespawn => pointToRespawn;
        public float Rotation => rotation;
        public SOConversationData Conversation => conversation;
        
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