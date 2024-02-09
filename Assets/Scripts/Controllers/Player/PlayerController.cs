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
    private float m_DistanceAttack = 5f;
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

    private void Start()
    {
        SelectSphere m_SelectSphere =  Resources.Load<SelectSphere>("GameObjects/SelectSphere");
    }
    public static PlayerController GetInstance() { return m_Instance; }

    public void AttackMode(bool atackMode)
    {

        if (atackMode) SpawnSelectSphere();
    }

    private void SpawnSelectSphere()
    {
        
    }
}
