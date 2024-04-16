using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameParametres;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float m_Speed = 10;
    [SerializeField]
    private float m_GroundCheckDistance;
    [SerializeField]
    private Transform m_Foot;
    [SerializeField]
    private Transform m_CameraTransform;
    
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;

    private CapsuleCollider m_Collider;
    private Vector3 m_ColliderHalfSize;
    private Vector3 m_SlopeNormalPerp;
    private float m_SlopeDownAngle;
    private float m_SlopeDownAngleOld;
    private bool m_IsInSlope;

    private bool m_IsPlaying;


    void Start()
    {
        m_IsPlaying = true;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();
        m_Collider = GetComponent<CapsuleCollider>();

        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
        GameStateEvent.Instance.SubscribeTo(EGameState.None, OnNoneMode);

       
        m_ColliderHalfSize = new Vector3(m_Collider.radius, m_Collider.height/2, m_Collider.radius);
    }

    void Update()
    {
        //Debug.DrawLine(transform.position, transform.forward * 1 + transform.position, Color.red);
        SloopCheck();
        Move();
        if (m_IsInSlope) MoveInSlope();
    }

    private void SloopCheck()
    {
        SloopCheckVertical();
    }

    private void SloopCheckHorizontal()
    {

    }

    private void SloopCheckVertical()
    {
        if (IsGrounded(out RaycastHit hit)) 
        {
            m_SlopeNormalPerp = Vector3.Cross(transform.right, hit.normal).normalized;
            m_SlopeDownAngle = Vector3.Angle(hit.normal, Vector3.up);
            //transform.forward = m_SlopeNormalPerp;

            /*if(m_SlopeDownAngle != m_SlopeDownAngleOld)
            {
                
            }*/

            m_IsInSlope = m_SlopeDownAngle != 0;

            m_SlopeDownAngleOld = m_SlopeDownAngle;
            Debug.Log($"ANGLE: {m_SlopeDownAngle}");

            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(hit.point, m_SlopeNormalPerp, Color.blue);
        }
        else
        {
            m_IsInSlope = false;
        }
    }

    private void MoveInSlope()
    {
        Vector3 velocity = m_Rigidbody.velocity;
        velocity.x = -m_SlopeNormalPerp.x * velocity.x;
        velocity.y = -m_SlopeNormalPerp.y * velocity.y;
        velocity.z = -m_SlopeNormalPerp.z * velocity.z;



        m_Rigidbody.velocity = velocity;
    }

    private void Move()
    {
        if (!m_IsPlaying) return;

        // obtient les valeurs des touches horizontales et verticales
        float hDeplacement = Input.GetAxis(GameParametres.InputName.AXIS_HORIZONTAL);
        float vDeplacement = Input.GetAxis(GameParametres.InputName.AXIS_VERTICAL);

        // TODO invers les axis si on pinte

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
        m_Animator.SetFloat(GameParametres.Animation.PLAYER_FLOAT_VELOCITY, velocity.magnitude);

        // Ignorer les changement 
        velocity.y = m_Rigidbody.velocity.y;

        m_Rigidbody.velocity = velocity;
    }

    private bool IsGrounded(out RaycastHit hit)
    {
        return Physics.Raycast(m_Foot.position, Vector3.down, out hit, m_GroundCheckDistance, LayerMaskValue.GROUND);
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
