using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectilCombo : MonoBehaviour
{
    private Vector3 m_Velocity = Vector3.zero;
    private event Action<ATargetController> m_OnHit;
    private ATargetController m_Target;

    public void Lauch(ATargetController target, float speed, Action<ATargetController> OnHit)
    {
        if (target == null) return;
        m_Target = target;
        m_Velocity = (m_Target.transform.position - transform.position).normalized * speed;
        m_OnHit += OnHit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ATargetController>(out ATargetController target) && target == m_Target)
        {
            m_OnHit?.Invoke(target);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Translate(m_Velocity * Time.deltaTime);
    }



}
