using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TestCameraChanger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Camera[] cameras;

    [SerializeField]
    private Camera defaultCamera;

    private int index = 0;

    private void Start()
    {
        TestControls inputs = new();
        inputs.Testing.SwitchCamera.performed += (CallbackContext _) => Next();
        inputs.Enable();

        DisableAllCameras();
        SetCamera(index);
    }

    public void Next()
    {
        index = (index + 1) % cameras.Length;
        DisableAllCameras();
        SetCamera(index);
    }

    private void DisableAllCameras()
    {
        foreach (Camera c in cameras)
        {
            c.enabled = false;
        }
    }

    private void SetCamera(int index)
    {
        cameras[index].enabled = true;
    }
}
