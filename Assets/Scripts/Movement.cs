
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
        speed = 0f;
    }

    public void StartPlayer()
    {
        speed = 5f;
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
