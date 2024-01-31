using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SelectSphere : MonoBehaviour
{
    private float m_Radius = 100f;
    private MeshRenderer m_Renderer;

    private static SelectSphere m_Instance;

    public static SelectSphere INSTANCE
    {
        get
        {
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;

    }

    private void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
       //  enabled = false;

    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Transform playerTransform = PlayerController.INSTANCE.transform;

        transform.position = playerTransform.position;
    }

    public void DrawSphere(float radius)
    {
        enabled = true;
        m_Radius = radius;
        transform.localScale = new Vector3(m_Radius, m_Radius, m_Radius);
    }

    public void HideSphere() 
    {
        enabled = false;
    }
}
