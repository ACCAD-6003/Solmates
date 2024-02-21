using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPolish : MonoBehaviour
{
    public Transform LookAt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(LookAt);

        float r = transform.parent.GetComponent<Movement2>().RadiusVector().magnitude;

        transform.localPosition = Vector3.Scale(transform.localPosition, new Vector3(1, 1, 0)) + -Vector3.forward * (0.25f * r + 6) ;
    }
}
