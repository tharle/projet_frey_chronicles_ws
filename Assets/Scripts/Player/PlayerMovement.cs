using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameParametres;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float m_Speed = 10;
    [SerializeField] private float m_GroundCheckDistance;
    [SerializeField] private Vector2 m_SlopeVerticalSpeedBounds;
    [SerializeField] private Transform m_Foot;
    [SerializeField] private Transform m_CameraTransform;
 
    private Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private bool IsOnSloop()
    {
        if (Physics.Raycast(m_Foot.position, Vector3.down, out RaycastHit hit, m_GroundCheckDistance, LayerMaskValue.GROUND)) 
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle != 0;
        }

        return false;
    }

    private bool IsGrounded(out RaycastHit hit)
    {
        return Physics.Raycast(m_Foot.position, Vector3.down, out hit, m_GroundCheckDistance, LayerMaskValue.GROUND);
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        return IsGrounded(out hit);
    }

    public void Move()
    {
        // obtient les valeurs des touches horizontales et verticales
        float hDeplacement = Input.GetAxis(GameParametres.InputName.AXIS_HORIZONTAL);
        float vDeplacement = Input.GetAxis(GameParametres.InputName.AXIS_VERTICAL);


        //obtient la nouvelle direction ( (avant/arrièrre) + (gauche/droite) )
        Vector3 directionDep = m_CameraTransform.forward * vDeplacement + m_CameraTransform.right * hDeplacement;
        directionDep.y = 0; //pas de valeur en y , le cas où la caméra regarde vers le bas ou vers le haut

        Vector3 velocity = Vector3.zero;
        if (directionDep != Vector3.zero) //change de direction s’il y a un changement
        {
            //Oriente le personnage vers la direction de déplacement et applique la vélocité dans la même direction
            transform.forward = directionDep;
            velocity = directionDep * m_Speed;

        }

        PlayerAnimation.Instance?.ChangeVelocity(velocity.magnitude);

        // Ignorer les changement 
        velocity.y = m_Rigidbody.velocity.y;
        m_Rigidbody.useGravity = true;
        if (IsOnSloop()) 
        {
            m_Rigidbody.useGravity = false;
            velocity.y = velocity.y < 0 && velocity.y > m_SlopeVerticalSpeedBounds.x ? m_SlopeVerticalSpeedBounds.x : velocity.y;
            velocity.y = velocity.y > 0 && velocity.y < m_SlopeVerticalSpeedBounds.y ? m_SlopeVerticalSpeedBounds.y : velocity.y;
        }
        else if(!IsGrounded())
        {
            velocity.y = Physics.gravity.y;
        }

        m_Rigidbody.velocity = velocity;
    }

    public void StartMoving()
    {
        m_Rigidbody.useGravity = true;
    }

    public void StopMove()
    {
        m_Rigidbody.useGravity = false;
        m_Rigidbody.velocity = Vector3.zero;
    }

}
