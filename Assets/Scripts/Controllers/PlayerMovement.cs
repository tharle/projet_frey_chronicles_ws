using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float m_Velocity = 10;

    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float axisHorizontal = Input.GetAxis(GameParametres.InputName.AXIS_HORIZONTAL);
        float axisVertical = Input.GetAxis(GameParametres.InputName.AXIS_VERTICAL);


        Vector3 velocity = m_Rigidbody.velocity;
        velocity.x = axisHorizontal * m_Velocity;
        velocity.z = axisVertical * m_Velocity;

        m_Rigidbody.velocity = velocity;

    }
}
