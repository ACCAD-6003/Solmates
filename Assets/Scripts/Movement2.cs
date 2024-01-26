using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public GameObject otherPlayer;
    [SerializeField]
    public SpringJoint joint;

    [SerializeField]
    public InputActionAsset input;
    public InputActionMap actionMap;

    // <0 is clockwise, >0 is counterclockwise, 0 is not moving 
    private float movementDirection = 0;
    private float radiusDirection = 0;

    // Start is called before the first frame update
    void Start()
    {
        joint.minDistance = initialRadius;
        joint.maxDistance = initialRadius;

        actionMap = input.FindActionMap("Movement");
        actionMap.actionTriggered += ActionTriggered;
        actionMap.Enable();
    }

    private void ActionTriggered(InputAction.CallbackContext inpAction)
    {
        string action = inpAction.action.name;

        if (action == "Circle")
        {
            movementDirection = inpAction.ReadValue<float>();
        }
        else if (action == "Radius")
        {
            if (IsNearGrowShrinkStatue()) {
                radiusDirection = inpAction.ReadValue<float>();
            }
        }
    }

    private Vector3 RadiusVector()
    {
        return Vector3.Scale(new Vector3(1, 0, 1), otherPlayer.transform.position - transform.position);
    }

    public void Lock()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    public void Unlock()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    private bool IsNearGrowShrinkStatue()
    {
        GameObject[] statues = GameObject.FindGameObjectsWithTag("Totem");

        float interactRadius = 2f;

        foreach(GameObject statue in statues)
        {
            if (Mathf.Abs((transform.position - statue.transform.position).magnitude) < interactRadius) {
                return true;
            }
        }

        return false;
    }

    private void ChangeRadius(float delta)
    {
        float newRadius = Mathf.Clamp(joint.maxDistance + delta, minRadius, maxRadius);

        joint.maxDistance = newRadius;
        joint.minDistance = newRadius;
    }

    private void Update()
    {
        if (movementDirection == 0)
        {
            Lock();
        } 
        else
        {
            Unlock();
        }

        GetComponent<Rigidbody>().velocity = movementDirection * moveSpeed * Vector3.Cross(Vector3.up, Vector3.Normalize(RadiusVector())) * RadiusVector().magnitude;

        if (radiusDirection != 0)
        {
            Lock();
            ChangeRadius(radiusDirection * Time.deltaTime * pullPushSpeed);
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
