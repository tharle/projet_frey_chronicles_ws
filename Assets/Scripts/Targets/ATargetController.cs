using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATargetController : MonoBehaviour
{
    Renderer m_Renderer;
    private Color m_DefaultColor;
    private Color m_InRangeColor;
    private Color m_SelectedColor;

    private bool m_IsSelected;

    public abstract ITarget GetTarget();

    public virtual bool IsSelected { 
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
        AfterStart();
    }

    protected virtual void AfterStart() { }

    public virtual void ShowSelected()
    {
        m_Renderer.material.color = m_SelectedColor;
    }

    public virtual void DesSelected()
    {
        m_Renderer.material.color = m_InRangeColor;
    }
}
