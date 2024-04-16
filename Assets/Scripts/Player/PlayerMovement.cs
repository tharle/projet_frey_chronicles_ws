using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float m_Speed = 10;

    [SerializeField]
    private Transform m_CameraTransform;
    
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
        Debug.DrawLine(transform.position, transform.forward * 1 + transform.position, Color.red);
        Move();
    }

    private void Move()
    {
        if (!m_IsPlaying) return;

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
        m_Animator.SetFloat(GameParametres.Animation.PLAYER_FLOAT_VELOCITY, velocity.magnitude);

        // Ignorer les changement 
        velocity.y = m_Rigidbody.velocity.y;

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
