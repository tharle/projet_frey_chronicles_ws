using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private PlayerController m_Player;
    private GameStateController m_StateController;
    private bool m_AtackMode = false;

    private void Start()
    {
        m_Player = PlayerController.GetInstance();
        m_StateController = GetComponent<GameStateController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_AtackMode = !m_AtackMode;


            m_Player.AttackMode(m_AtackMode);
        }
    }

}
