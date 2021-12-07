using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    void LateUpdate()
    {
        //transform.LookAt(mainCamera.transform);  // Need to adjust the rotation of the texture along y-axis.
        transform.forward = Camera.main.transform.forward; // This is another way, and no need to adjust the rotation of texture along y-axis.
    }
}
