using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Movement2 : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;
    [SerializeField]
    public float pullPushSpeed = 5f;

    [SerializeField]
    public float initialRadius = 5;
    [SerializeField]
    public float minRadius = 2;
    [SerializeField]
    public float maxRadius = 20;

    [SerializeField]
    public int player = 1;
    private int device;

    [SerializeField]
    public GameObject otherPlayer;
    [SerializeField]
    public SpringJoint joint;

    List<string> AllowedDevices = new()
    {
        "XInputControllerWindows",
        "HID::Logitech Logitech RumblePad 2 USB"
    };

    [SerializeField]
    public InputActionAsset input;
    public InputActionMap actionMap;

    private float currentSpeed;
    private Rigidbody rb;

    // <0 is clockwise, >0 is counterclockwise, 0 is not moving 
    public float MovementDirection { get; private set; }
    public float RadiusDirection { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        joint.minDistance = initialRadius;
        joint.maxDistance = initialRadius;
        currentSpeed = moveSpeed;

        SetPlayerControls();
        actionMap = input.FindActionMap("Movement");
        actionMap.actionTriggered += ActionTriggered;
        actionMap.Enable();
    }

    private void SetPlayerControls()
    {
        int current = 0;

        foreach(var d in InputSystem.devices)
        {
            if(AllowedDevices.Contains(d.layout))
            {
                current++;
            }

            if (current == player)
            {
                device = d.deviceId;
            }
        }
    }

    private void ActionTriggered(InputAction.CallbackContext inpAction)
    {
        if (inpAction.control.device.deviceId != device)
        {
            return;
        }

        string action = inpAction.action.name;

        if (action == "Circle")
        {
            MovementDirection = inpAction.ReadValue<float>();
        }
        else if (action == "Radius")
        {
            if (IsNearGrowShrinkStatue()) {
                RadiusDirection = inpAction.ReadValue<float>();
            }

            if (inpAction.ReadValue<float>() == 0)
            {
                RadiusDirection = 0;
            }
        }
    }

    public Vector3 RadiusVector()
    {
        return Vector3.Scale(new Vector3(1, 0, 1), otherPlayer.transform.position - transform.position);
    }

    public void Lock()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    public void Unlock()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Freeze()
    {
        currentSpeed = 0;
    }
    
    public void Unfreeze()
    {
        currentSpeed = moveSpeed;
    }

    private bool IsNearGrowShrinkStatue()
    {
        GameObject[] statues = GameObject.FindGameObjectsWithTag("Totem");

        float interactRadius = 2f;

        foreach(GameObject statue in statues)
        {
            if (Mathf.Abs(Vector3.Scale(transform.position - statue.transform.position, new Vector3(1,0,1)).magnitude) < interactRadius) {
                return true;
            }
        }

        return false;
    }

    public void ChangeRadius(float delta)
    {
        float newRadius = Mathf.Clamp(joint.maxDistance + delta, minRadius, maxRadius);

        joint.maxDistance = newRadius;
        joint.minDistance = newRadius;
    }

    public void StopGrowing()
    {
        RadiusDirection = 0;
    }

    private void Update()
    {
        if (MovementDirection == 0)
        {
            Lock();
        } 
        else
        {
            Unlock();
        }

        rb.velocity = Vector3.Scale(Vector3.Cross(Vector3.up, Vector3.Normalize(RadiusVector())) * (MovementDirection * currentSpeed), new Vector3(1, 0, 1)) + Mathf.Min(0, rb.velocity.y) * Vector3.up;

        if (RadiusDirection != 0)
        {
            Lock();
            ChangeRadius(RadiusDirection * Time.deltaTime * pullPushSpeed);
        }
        else
        {
            Unlock();
        }
    }

    private void FixedUpdate()
    {
        transform.LookAt(otherPlayer.transform);
    }
}
