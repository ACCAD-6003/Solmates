using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughShaderTracker : MonoBehaviour
{
    private static readonly int Size = Shader.PropertyToID("size");
    
    [SerializeField] Camera playerCamera;
    [SerializeField] private Material material;
    [SerializeField] LayerMask mask;
    

    private void Update()
    {
        var rayDirection = (playerCamera.transform.position - transform.position).normalized;
        
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, Mathf.Infinity, mask))
        {
            ToggleSeeThrough(true);
        }
        else
        {
            ToggleSeeThrough(false);
        }
    }
    
    private void ToggleSeeThrough(bool shouldSeeThrough)
    {
        material.SetFloat(Size, shouldSeeThrough ? 1.5f : 0);
    }
}
