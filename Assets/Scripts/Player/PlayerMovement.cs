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
    private Animator m_Animator;

    private bool m_IsPlaying;

    void Start()
    {
        m_IsPlaying = true;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();

        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
        GameStateEvent.Instance.SubscribeTo(EGameState.None, OnNoneMode);
    }

    void Update()
    {
        if (!m_IsPlaying) return;

        Move(); 
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

    private void Move()
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

        // TODO : Creer une classe pour gérer les animations
        m_Animator.SetFloat(GameParametres.AnimationPlayer.FLOAT_VELOCITY, velocity.magnitude);

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

    private void OnInterractionMode(bool isEnterState)
    {
        if (isEnterState) StopMove();
    }

    private void OnNoneMode(bool isEnterState)
    {
        if (isEnterState) StartPlaying();
    }

    private void StopMove()
    {
        m_IsPlaying = false;
        m_Rigidbody.useGravity = false;
        m_Rigidbody.velocity = Vector3.zero;
    }

    private void StartPlaying()
    {
        m_IsPlaying = true;
        m_Rigidbody.useGravity = true;
    }

}
