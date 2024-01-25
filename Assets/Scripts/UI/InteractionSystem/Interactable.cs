using System;
using Controller;
using UnityEngine;
using UnityEngine.Events;

namespace UI.InteractionSystem
{
    public class Interactable : MonoBehaviour
    {
        private const string PLAYER_ONE_TAG = "PlayerOne";
        private const string PLAYER_TWO_TAG = "PlayerTwo";
        
        [SerializeField] private UnityEvent onInRange;
        [SerializeField] private UnityEvent onOutOfRange;
        [SerializeField] private UnityEvent onPlayerOneInteract;
        [SerializeField] private UnityEvent onPlayerTwoInteract;

        private int playersInRange;

        private static bool IsPlayer(GameObject other)
        {
            return HasTag(other, PLAYER_ONE_TAG) || HasTag(other, PLAYER_TWO_TAG);
        }
        
        private static bool HasTag(GameObject objectToCheck, string tagToCheck)
        {
            return objectToCheck.gameObject.CompareTag(tagToCheck);
        } 
        
        private void OnTriggerEnter(Collider other)
        {
            if (!IsPlayer(other.gameObject)) return;
            
            if (HasTag(other.gameObject, PLAYER_ONE_TAG)) UIController.OnInteract += onPlayerOneInteract.Invoke;
            else if (HasTag(other.gameObject, PLAYER_TWO_TAG)) UIController.OnPlayerTwoInteract += onPlayerTwoInteract.Invoke;
            
            if (playersInRange == 0) onInRange.Invoke();
            playersInRange++;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!IsPlayer(other.gameObject)) return;
            
            if (HasTag(other.gameObject, PLAYER_ONE_TAG)) UIController.OnInteract -= onPlayerOneInteract.Invoke;
            else if (HasTag(other.gameObject, PLAYER_TWO_TAG)) UIController.OnPlayerTwoInteract -= onPlayerTwoInteract.Invoke;
            
            playersInRange--;
            if (playersInRange == 0) onOutOfRange.Invoke();
        }

        private void OnDestroy()
        {
            UIController.OnInteract -= onPlayerOneInteract.Invoke;
            UIController.OnPlayerTwoInteract -= onPlayerTwoInteract.Invoke;
        }
    }
}