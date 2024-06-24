using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SelectSphere : MonoBehaviour
{
    private float m_SphereRadius;
    private float m_SphereRadiusMin = 0.1f;
    private float m_SphereRadiusMax = 0.1f;
    private ATargetController m_TargetSelected;

    [SerializeField] private GameObject m_Model;

    private static SelectSphere m_Instance;
    public static SelectSphere Instance{ 
        get { 
            return m_Instance; 
        } 
    }

    private void Awake()
    {
        if (m_Instance != null) Destroy(gameObject);

        m_Instance = this;
    }

    private void Start()
    {
        SubscribeAll();
    }

    private void SubscribeAll()
    {
        PlayerController.Instance.OnSpell += OnSpell;
        GameEventSystem.Instance.SubscribeTo(EGameEvent.SelectTarget, OnSelectTarget);
    }

    private void OnSpell(int damage, EElemental elementalId)
    {
        m_TargetSelected.ReciveSpell(damage, elementalId);
    }

    private void OnSelectTarget(GameEventMessage message)
    {
        if(message.Contains<ATargetController>(EGameEventMessage.TargetController, out ATargetController targetController))
        {
            SelectTarget(targetController);
        }
    }


    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Transform playerTransform = PlayerController.Instance.transform;

        transform.position = playerTransform.position;
    }

    public bool IsTargetSelected()
    {
        return m_TargetSelected != null && m_TargetSelected.IsSelected;
    }

    private void SelectTarget(ATargetController target)
    {
        if (IsTargetSelected()) 
        {
            m_TargetSelected?.DesSelected();
            m_TargetSelected = target;
        }

        m_TargetSelected?.ShowSelected();
    }

    public void ShowSphere(float radius)
    {
        //transform.localScale = new Vector3(m_SphereRadius, m_SphereRadius, m_SphereRadius);   
        m_SphereRadiusMax = radius;
        m_SphereRadius = m_SphereRadiusMax;
        DrawSphere(); // TODO: Ajouter animation sphere_enable ou Lerp
        m_Model.SetActive(true);
    }
    public void DespawnSphere() 
    {
        m_SphereRadius = m_SphereRadiusMin;
        if(m_TargetSelected != null && !m_TargetSelected.IsDestroyed()) m_TargetSelected.ClearSelected();
        m_TargetSelected = null;
        //DrawSphere(); // TODO: Ajouter animation sphere_disable ou Lerp
        HideSphere();
    }

public void HideSphere()
    {
        m_Model.SetActive(false);
    }

    private void DrawSphere()
    {
        transform.localScale = new Vector3(m_SphereRadius/2, m_SphereRadius/2, m_SphereRadius/2);
    }
}
