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

    private bool m_IsMoving;


    void Start()
    {
        m_IsMoving = true;
        m_Rigidbody = GetComponent<Rigidbody>();

        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.forward * 1 + transform.position, Color.red);
        Move();
    }

    private void Move()
    {
        if (!m_IsMoving) return;

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

        m_Rigidbody.velocity = velocity;
    }

    private void OnInterractionMode(bool isEnterState)
    {

        m_IsMoving = !isEnterState;

        if (!m_IsMoving) StopMove();
    }

    private void StopMove()
    {
        m_Rigidbody.velocity = Vector3.zero;
    }
}
