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
            
            if (playersInRange == 0)
            {
                onInRange.Invoke();
                UIController.OnInteract += onInteract.Invoke;
            }
            
            playersInRange++;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!HasTag(other.gameObject, PLAYER_TAG)) return;
            
            playersInRange--;
            if (playersInRange == 0)
            {
                onOutOfRange.Invoke();
                UIController.OnInteract -= onInteract.Invoke;
            }
        }

        private void OnDestroy()
        {
            UIController.OnInteract -= onInteract.Invoke;
        }
    }
}