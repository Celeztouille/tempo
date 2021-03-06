using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToCam : MonoBehaviour
{

    [SerializeField] Transform cameraTr;

    void Update()
    {
        if (transform.forward != cameraTr.forward)
        {
            transform.rotation = Quaternion.FromToRotation(transform.forward, cameraTr.forward);
        }
    }
}
