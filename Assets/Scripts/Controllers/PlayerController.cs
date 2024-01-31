using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController m_Instance;

    private bool m_AtackMode = false;

    public static PlayerController INSTANCE {
        get {
            return m_Instance;
        }
    }

    private void Awake()
    {
        if(m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_AtackMode = !m_AtackMode;

            //if (m_AtackMode) SelectSphere.INSTANCE.DrawSphere(100);
            // else SelectSphere.INSTANCE.HideSphere();
        }
    }

}
