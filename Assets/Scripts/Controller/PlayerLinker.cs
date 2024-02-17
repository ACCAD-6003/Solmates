using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controller
{
    public class PlayerLinker : MonoBehaviour
    {
        [SerializeField, ReadOnly] private bool inGameplay = true;
        
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
        
        private void DisablePlayerMovement()
        {
            inGameplay = false;
            Array.ForEach(FindObjectsOfType<Movement2>(true), m => m.Freeze());
        }
        
        private void EnablePlayerMovement()
        {
            inGameplay = true;
            Array.ForEach(FindObjectsOfType<Movement2>(true), m => m.Unfreeze());
        }
    }
}