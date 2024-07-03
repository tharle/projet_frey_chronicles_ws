using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeField] private GameObject m_Arrow;
    [SerializeField] private GameObject m_Block;

    private void Start()
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(GameParametres.TagName.PLAYER))
        {
            DungeonRoomManager.Instance.GoToNextRoom();
        }
    }
}
