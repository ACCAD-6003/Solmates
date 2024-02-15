using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    Transform ToCopy;

    [SerializeField]
    Vector3 Offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = ToCopy.position + Offset;
    }
}
