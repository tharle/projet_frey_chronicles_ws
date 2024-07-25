using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_MainCamera;
    [SerializeField] private CinemachineVirtualCamera m_SpellCamera;


    private void Start()
    {
        SubscribeAll();
    }

    

    private void OnDestroy()
    {
        UnsubscribeAll();
    }

    private void SubscribeAll()
    {
        GameStateEvent.Instance.SubscribeTo(EGameState.CastSpell, OnCastSpell);
    }

    private void UnsubscribeAll()
    {
        GameStateEvent.Instance.UnsubscribeFrom(EGameState.CastSpell, OnCastSpell);
    }

    private void OnCastSpell(bool enter)
    {
        m_SpellCamera.Priority = enter? 11 : 0;
    }
}
