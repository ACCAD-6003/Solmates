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
    }
}
