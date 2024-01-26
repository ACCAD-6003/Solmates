using System;
using UnityEngine;

namespace Controller
{
    public class PlayerLinker : MonoBehaviour
    {
        private void OnEnable()
        {
            UIController.Instance.OnSwapToUI += DisablePlayerMovement;
            UIController.Instance.OnSwapToGameplay += EnablePlayerMovement;
        }
        
        private void OnDisable()
        {
            UIController.Instance.OnSwapToUI -= DisablePlayerMovement;
            UIController.Instance.OnSwapToGameplay -= EnablePlayerMovement;
        }
        
        private static void DisablePlayerMovement()
        {
            Array.ForEach(FindObjectsOfType<Movement2>(true), m => m.Lock());
        }
        
        private static void EnablePlayerMovement()
        {
            Array.ForEach(FindObjectsOfType<Movement2>(true), m => m.Unlock());
        }
    }
}