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
            Array.ForEach(FindObjectsOfType<Movement>(true), m => m.StopPlayer());
        }
        
        private static void EnablePlayerMovement()
        {
            Array.ForEach(FindObjectsOfType<Movement>(true), m => m.StartPlayer());
        }
    }
}