using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    /**
     * **********************************************
     * Variables de configuration du Player 
     * **********************************************
     */
    [SerializeField] private float m_DistanceAttack = 5f;
    public float GetDistanceAttack() 
    { 
        return m_DistanceAttack;
    }

    private static PlayerController m_Instance;
    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
    }

    public static PlayerController GetInstance() { return m_Instance; }

    public void ShowInterractionMode(bool interractMode)
    {

        if (interractMode) SpawnSelectSphere();
        else DespawnSelectSphere();
    }

    private void SpawnSelectSphere()
    {
        SelectSphere.GetInstance().ShowSphere(m_DistanceAttack);
    }

    private void DespawnSelectSphere()
    {
        SelectSphere.GetInstance().HideSphere();
    }
}
