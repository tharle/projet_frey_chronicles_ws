using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector2 m_Delta;
    private void Update()
    {
        m_Delta = Vector2.zero;
        if (Input.GetKey("q")) m_Delta.x = 1;
    }
}
