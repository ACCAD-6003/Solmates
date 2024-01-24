
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

    private float GetAngleToOtherPlayer()
    {
        return Mathf.Atan2(transform.position.z - otherPlayer.position.z, transform.position.x - otherPlayer.position.x);
    }

    private void SetAngleToOtherPlayer(float angle)
    {
        transform.position = otherPlayer.position + radius * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
    }

    // Update is called once per frame
    void Update()
    {
        if (movementDirection != 0)
        {
            SetAngleToOtherPlayer(GetAngleToOtherPlayer() + movementDirection * speed * Time.deltaTime * 2 * Mathf.PI);
        }

        transform.LookAt(otherPlayer);
    }
}
