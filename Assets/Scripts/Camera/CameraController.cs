using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    
    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        Vector3 directionRotation = Vector3.zero;
        if (Input.GetKey(KeyCode.Q)) directionRotation = Vector3.up;
        if (Input.GetKey(KeyCode.E)) directionRotation = Vector3.down;

        transform.Rotate(directionRotation, Space.World);
    }
}
