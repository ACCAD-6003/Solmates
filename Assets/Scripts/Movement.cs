
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    [SerializeField]
    public float speed = 5f;
    [SerializeField]
    public float radius = 5;
    public float minRadius = 2;
    public float maxRadius = 20;
    public string totemAction;
    [SerializeField]
    public Transform otherPlayer;

    [SerializeField]
    public InputActionAsset input;
    public InputActionMap actionMap;

    private float savedSpeed = 0;

    // <0 is clockwise, >0 is counterclockwise, 0 is not moving 
    private float movementDirection = 0;

    // Start is called before the first frame update
    void Start()
    {
        actionMap = input.FindActionMap("Movement");
        actionMap.actionTriggered += ActionTriggered;
        actionMap.Enable();
        
        totemAction = "null";

        // normalize radius when game starts
        SetAngleToOtherPlayer(GetAngleToOtherPlayer());
    }

    private void ActionTriggered(InputAction.CallbackContext inpAction)
    {
        string action = inpAction.action.name;

        if (action == "Circle")
        {
            Circle(inpAction.ReadValue<float>());
        }
    }

    public void TotemActionTriggered(InputAction.CallbackContext inpAction)
    {
        string action = inpAction.action.name;

        if (action == "Grow" || action == "Shrink")
        {
            totemAction = action;
        }
    }

    private void Circle(float direction)
    {
        movementDirection = direction;
    }

    public void ChangeRadius(float scale)
    {
        radius = Mathf.Clamp(radius + scale, minRadius, maxRadius);
    }
    
    public void StopPlayer()
    {
        if (speed == 0) {
            return;
        }

        savedSpeed = speed;
        speed = 0f;
    }

    public void StartPlayer()
    {
        speed = savedSpeed;
    }

    private float GetAngleToOtherPlayer()
    {
        return Mathf.Atan2(transform.position.z - otherPlayer.position.z, transform.position.x - otherPlayer.position.x);
    }

    private void SetAngleToOtherPlayer(float angle)
    {
        transform.position = Vector3.Scale(otherPlayer.position, new Vector3(1,0,1))  + radius * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) + transform.position.y * Vector3.up;
    }

    private Vector3 RadiusVector() 
    {
        return Vector3.Scale(new Vector3(1,0,1), otherPlayer.position - transform.position);    
    }

    private void move(float localVelocity) {
        GetComponent<Rigidbody>().velocity = localVelocity * Vector3.Cross(Vector3.up, RadiusVector());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move(movementDirection * speed);
        SetAngleToOtherPlayer(GetAngleToOtherPlayer());
    }

    void Update() 
    {    
        transform.LookAt(otherPlayer);
    }
}
