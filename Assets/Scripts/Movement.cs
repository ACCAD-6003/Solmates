
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    public float speed = 5f;
    [SerializeField]
    public float radius = 5;
    [SerializeField]
    public Transform otherPlayer;

    [SerializeField]
    public InputActionAsset input;

    private float savedSpeed = 0;

    // <0 is clockwise, >0 is counterclockwise, 0 is not moving 
    private float movementDirection = 0;

    // Start is called before the first frame update
    void Start()
    {
        InputActionMap actionMap = input.FindActionMap("Movement");
        actionMap.actionTriggered += ActionTriggered;
        actionMap.Enable();

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

    private void Circle(float direction)
    {
        movementDirection = direction;
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
