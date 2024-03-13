using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ATargetController : MonoBehaviour
{
    Renderer m_Renderer;
    private Color m_DefaultColor;
    private Color m_InRangeColor;
    private Color m_SelectedColor;

    private bool m_IsSelected;
    public virtual bool IsSelected { 
        get { return m_IsSelected; }
        set
        {
            m_IsSelected = value;
            if (m_IsSelected) m_Renderer.material.color = m_InRangeColor;
            else m_Renderer.material.color = m_DefaultColor;
        }
    }

    public abstract ITarget GetTarget();

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

    public virtual void ClearSelected()
    {
        if(!gameObject.IsDestroyed() && GetTarget().IsAlive()) m_Renderer.material.color = m_DefaultColor;
    }

    protected virtual void TargetDie()
    {
        Destroy(gameObject, 0.1f);// TODO: add animation die
        DungeonTargetManager.Instance.TargetDie(this);
    }

    public virtual int ReciveAttack(int value)
    {
        return 0;
    }

    public virtual void ReciveSpell(int value, EElemental elementalId)
    {
        // TODO: change apres le mechaniques des spells
    }

    public virtual bool IsInRange(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        //Debug.Log(distance);
        return distance <= GetRange();
    }

    protected virtual float GetRange()
    {
        return GetTarget().GetRange();
    }

    public static T ConvertTo<T>(ATargetController target) where T : ATargetController
    {
        return (T)target;
    }
}
