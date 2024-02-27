using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class DoorController : MonoBehaviour
{
    private static readonly int VerticalOffset = Shader.PropertyToID("_Vertical_Offset");
    
    [SerializeField] bool requiresBothButtons = false;
    [SerializeField] bool doorStayOpen = false;
    [SerializeField] bool doorIsOpen = false;

    [Header("Door Animation Values")] 
    [SerializeField] private bool animates;
    [SerializeField, ShowIf("animates")] private MeshRenderer doorRenderer;
    [SerializeField, ShowIf("animates")] private Collider doorCollider;
    [SerializeField, ShowIf("animates")] private float doorOpenAmount = 4f;
    [SerializeField, ShowIf("animates")] private float doorClosedAmount = 10f;
    [SerializeField, ShowIf("animates")] private float changeRate = 1f;
    
    public ButtonController[] buttons;
    private bool DoorIsOpen {
        get => doorIsOpen;
        set
        {
            doorIsOpen = value;
            UpdateAnimator();
        }
    }
    
    private void OnEnable()
    {
        if (animates)
        {
            doorRenderer.material.SetFloat(VerticalOffset, doorIsOpen ? doorOpenAmount : doorClosedAmount);
            
            StartCoroutine(HandleDoorAnimation());
        }
    }

    private void UpdateAnimator()
    {
        if (!animates)
        {
            gameObject.SetActive(!DoorIsOpen);
        }
    }

    private IEnumerator HandleDoorAnimation()
    {
        var currentAmount = doorIsOpen ? doorOpenAmount : doorClosedAmount;
        while (true)
        {
            Debug.Log("Strating");
            doorCollider.enabled = doorCollider.enabled || !doorIsOpen;
            while (Mathf.Abs(currentAmount - (doorIsOpen ? doorOpenAmount : doorClosedAmount)) > 0.01f)
            {
                currentAmount += doorIsOpen ? changeRate * Time.deltaTime : -changeRate * Time.deltaTime;
                Debug.Log(currentAmount);
                currentAmount = Mathf.Clamp(currentAmount, doorClosedAmount, doorOpenAmount);
                doorRenderer.material.SetFloat(VerticalOffset, currentAmount);
                yield return null;
            }

            var lastValue = doorIsOpen;
            doorCollider.enabled = !doorIsOpen;
            yield return new WaitUntil(() => doorIsOpen != lastValue);
        }
    }
    
    public void CheckIfDoorShouldOpen()
    {
        bool allButtonsPressed = true;
        foreach (ButtonController button in buttons)
        {
            if (button.isPressed)
            {
                if (!requiresBothButtons)
                {
                    DoorIsOpen = true;
                }
            }
            else
            {
                if (requiresBothButtons)
                {
                    allButtonsPressed = false;
                }
            }
        }

        if (allButtonsPressed && requiresBothButtons)
        {
            DoorIsOpen = true;
        }
    }

    public void CheckIfDoorShouldClose()
    {
        foreach (ButtonController button in buttons)
        {
            if (button.isPressed)
            {
                DoorIsOpen = true;
                break;
            }
            else
            {
                if (!doorStayOpen && DoorIsOpen)
                {
                    DoorIsOpen = false;
                }
            }
        }
    }
}