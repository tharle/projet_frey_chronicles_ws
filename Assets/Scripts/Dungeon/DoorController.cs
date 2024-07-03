using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeField] private GameObject m_Arrow;
    [SerializeField] private GameObject m_Block;
    [SerializeField] private Animator m_Animator;
    private bool m_IsOpen = false;

    private void Start()
    {
        m_Arrow.SetActive(true);
        m_Block.SetActive(false);
        SubscribeAll();
    }

    private void SubscribeAll()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.BattleMode, OnBattleMode);
    }

    private void OnBattleMode(GameEventMessage message)
    {
        if(message.Contains<bool>(EGameEventMessage.Enter, out bool enter))
        {
            m_Arrow.SetActive(!enter);
            m_Block.SetActive(enter);
        }
    }

    public void OpenDoor()
    {
        if (m_IsOpen) return;

        m_IsOpen = true;
        m_Animator.SetTrigger(GameParametres.AnimationDungeon.TRIGGER_OPEN_DOOR);
        Destroy(GetComponent<BoxCollider>(), 0.5f);
    }
}
