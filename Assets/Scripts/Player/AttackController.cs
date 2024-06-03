using System.Collections;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_Speed;

    private void Start()
    {
    }

    private void Update()
    {
        Vector3 newPosition =  Vector3.Lerp(transform.position, m_Target.position, Time.deltaTime);
        newPosition.y =  Mathf.Clamp(newPosition.y, 1, 50);
        transform.position = newPosition;   
    }

}
