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
    public float DistanceAttack { 
        get { return m_DistanceAttack; } 
        set {  m_DistanceAttack = value; } 
    }

    private static PlayerController m_Instance;
    public static PlayerController Instance { get { return m_Instance; } }

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
        EventSystem.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
    }

    private void OnInterractionMode(bool isEnterState)
    {

        if (isEnterState) SpawnSelectSphere();
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
