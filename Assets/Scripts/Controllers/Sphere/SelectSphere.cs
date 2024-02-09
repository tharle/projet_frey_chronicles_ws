using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SelectSphere : MonoBehaviour
{
    private float m_SphereRadius = 0.1f;
    private float m_SphereRadiusMax = 0.1f;
    private void Awake()
    {
        transform.localScale = new Vector3(m_SphereRadius, m_SphereRadius, m_SphereRadius);
    }

    public void SetSphereRadius(float sphereRadius) 
    {
        m_SphereRadiusMax = sphereRadius;

        //TODO Ajouter un courotine pour faire le lerp entre le sphre Radius actuel vers le Max
    }



    private void FollowPlayer()
    {
        Transform playerTransform = PlayerController.GetInstance().transform;

        transform.position = playerTransform.position;
    }

    private void DrawSphere(float radius)
    {
        //transform.localScale = new Vector3(m_SphereRadius, m_SphereRadius, m_SphereRadius);

        // TODO: Ajouter animation sphere_enable
        //m_Model.SetActive(true);
    }

    public void HideSphere() 
    {
        // TODO: Ajouter animation sphere_disable
        //m_Model.SetActive(false);

    }
}
