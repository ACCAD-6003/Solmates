using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UI.Dialogue_System;
using UnityEngine;

namespace Checkpoint_System
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private bool endSpot;
        [SerializeField, ShowIf("@!endSpot")] private Vector3 pointToRespawn;
        [SerializeField, ShowIf("@!endSpot")] private float rotation;
        [SerializeField, ShowIf("@!endSpot")] private SOConversationData conversation;

        public static Action<Checkpoint> OnCheckpointReached;

        public Vector3 PointToRespawn => pointToRespawn;
        public float Rotation => rotation;
        public SOConversationData Conversation => conversation;
        
        private readonly List<Collider> playersWhoReachedZone = new();

        private void OnEnable()
        {
            if(endSpot) Controller.UIController.OnSkipScene += HandleTransition;
        }

        [Button("DEBUG Jump To Point"), ShowIf("@!endSpot")]
        private void DebugJumpToPoint()
        {
            playersWhoReachedZone.Add(null);
            playersWhoReachedZone.Add(null);
            OnCheckpointReached?.Invoke(this);
        }

        private void HandleTransition()
        {
            int nextSceneIndex = SceneTools.NextSceneExists ? SceneTools.NextSceneIndex : 0;
            StartCoroutine(SceneTools.TransitionToScene(nextSceneIndex));
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !playersWhoReachedZone.Contains(other))
            {
                playersWhoReachedZone.Add(other);
                if(playersWhoReachedZone.Count == 2)
                {
                    if (endSpot)
                    {
                        HandleTransition();
                    }
                    else
                    {
                        OnCheckpointReached?.Invoke(this);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // make sure both players are in the end spot
            if (other.CompareTag("Player") && endSpot)
            {
                playersWhoReachedZone.Remove(other);
            }
        }

        private void OnDisable()
        {
            if(endSpot) Controller.UIController.OnSkipScene -= HandleTransition;
        }
    }
}