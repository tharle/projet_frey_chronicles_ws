using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Material m_Material; // temp

    private EnemyData.Enemy m_Enemy;
    public EnemyData.Enemy Enemy { set { m_Enemy = value; } }

    private Color m_DefaultColor;
    private Color m_SelectedColor;

    private bool m_IsSelected;
    public bool IsSelected
    {
        get { 
            return m_IsSelected; 
        }
        set
        {
            m_IsSelected = value;
            if(m_IsSelected) m_Material.color = m_SelectedColor;
            else m_Material.color = m_DefaultColor;
        }
    }

    private void Start()
    {
        m_Material = GetComponentInChildren<Renderer>().material;
        m_IsSelected = false;
        m_DefaultColor = m_Material.color;
        m_SelectedColor = Color.blue;
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
}
