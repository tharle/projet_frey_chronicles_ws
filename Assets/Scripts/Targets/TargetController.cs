using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetController : MonoBehaviour
{
    Renderer m_Renderer;
    private Color m_DefaultColor;
    private Color m_InRangeColor;
    private Color m_SelectedColor;

    private bool m_IsSelected;

    public bool IsSelected { 
        get { return m_IsSelected; }
        set
        {
            m_IsSelected = value;
            if (m_IsSelected) m_Renderer.material.color = m_InRangeColor;
            else m_Renderer.material.color = m_DefaultColor;
        }
    }

    private void Start()
    {
        m_Renderer = GetComponentInChildren<Renderer>();
        m_IsSelected = false;
        m_DefaultColor = m_Renderer.material.color;
        m_InRangeColor = Color.blue;
        m_SelectedColor = Color.yellow;
    }

    public float DistanceFrom(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition);
    }

    public bool IsInPlayerRange()
    {
        PlayerController player = PlayerController.Instance;
        float distance = DistanceFrom(player.transform.position);
        Debug.Log(distance);
        return DistanceFrom(player.transform.position) <= player.DistanceAttack;
    }

    public void ShowSelected()
    {
        m_Renderer.material.color = m_SelectedColor;
    }

    public void DesSelected()
    {
        m_Renderer.material.color = m_InRangeColor;
    }
}
