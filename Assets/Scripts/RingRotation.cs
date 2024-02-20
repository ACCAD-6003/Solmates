using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 1;
        float speed2 = -0.6f;
        float t = Time.time;
        float r = 0.15f;
        transform.LookAt(transform.position + new Vector3(r*Mathf.Sin(speed*t), 1, r*Mathf.Cos(speed*t)), new Vector3(Mathf.Sin(speed2*t), 0, Mathf.Cos(speed2*t)));
    }
}
