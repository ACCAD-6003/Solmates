using System;
using Controller;
using UnityEngine;
using UnityEngine.Events;

namespace UI.InteractionSystem
{
    public class Interactable : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";
        
        [SerializeField] private UnityEvent onInRange;
        [SerializeField] private UnityEvent onOutOfRange;
        [SerializeField] private UnityEvent onInteract;

        private int playersInRange;
        
        private static bool HasTag(GameObject objectToCheck, string tagToCheck)
        {
            return objectToCheck.gameObject.CompareTag(tagToCheck);
        } 
        
        private void OnTriggerEnter(Collider other)
        {
            if (!HasTag(other.gameObject, PLAYER_TAG)) return;
            
            UIController.OnInteract += onInteract.Invoke;
            
            if (playersInRange == 0) onInRange.Invoke();
            playersInRange++;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!HasTag(other.gameObject, PLAYER_TAG)) return;
            
            UIController.OnInteract -= onInteract.Invoke;
            
            playersInRange--;
            if (playersInRange == 0) onOutOfRange.Invoke();
        }

        private void OnDestroy()
        {
            UIController.OnInteract -= onInteract.Invoke;
        }
    }
}