using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SelectSphere : MonoBehaviour
{
    private float m_SphereRadius;
    private float m_SphereRadiusMin = 0.1f;
    private float m_SphereRadiusMax = 0.1f;
    private TargetController m_TargetSelected;

    public Action<ITarget> OnTargetSelected;

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

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Transform playerTransform = PlayerController.Instance.transform;

        transform.position = playerTransform.position;
    }

    public void SelectTarget(TargetController target)
    {
        if (m_TargetSelected != null) m_TargetSelected.DesSelected();
        m_TargetSelected = target;

        m_TargetSelected?.ShowSelected();
        OnTargetSelected?.Invoke(m_TargetSelected?.GetTarget());
    }

    public void ShowSphere(float radius)
    {
        //transform.localScale = new Vector3(m_SphereRadius, m_SphereRadius, m_SphereRadius);   
        m_SphereRadiusMax = radius;
        m_SphereRadius = m_SphereRadiusMax;
        DrawSphere(); // TODO: Ajouter animation sphere_enable ou Lerp
        m_Model.SetActive(true);
    }
    public void HideSphere() 
    {
        m_SphereRadius = m_SphereRadiusMin;
        DrawSphere(); // TODO: Ajouter animation sphere_disable ou Lerp
        m_Model.SetActive(false);

    }
    private void DrawSphere()
    {
        transform.localScale = new Vector3(m_SphereRadius/2, m_SphereRadius/2, m_SphereRadius/2);
    }
}
