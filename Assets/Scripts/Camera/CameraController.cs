using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private Transform m_LookAtDefault;
    private CinemachineVirtualCamera m_Camera;
    private Transform m_LookAtCurrent;

    private void Start()
    {
        m_Camera = GetComponent<CinemachineVirtualCamera>();
        m_LookAtDefault = m_Camera.LookAt;
        m_LookAtCurrent = m_LookAtDefault;

        SubscribeAllEvents();
    }

    private void SubscribeAllEvents()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.SelectTarget, OnSelectTarget);
        GameStateEvent.Instance.SubscribeTo(EGameState.None, OnStateNone);
    }

    private void OnStateNone(bool enter)
    {
        if (enter) 
        {
            m_LookAtCurrent = m_LookAtDefault;
            RefeshCamera();
        } 
    }

    private void OnSelectTarget(GameEventMessage message)
    {
       if(message.Contains<ATargetController>(EGameEventMessage.TargetController, out ATargetController targetController))
        {
            m_LookAtCurrent = targetController == null? m_LookAtDefault: targetController.transform;
            RefeshCamera();
        }

    }

    private void RefeshCamera()
    {
        m_Camera.LookAt = m_LookAtCurrent;
        m_Camera.Follow = m_LookAtCurrent;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        Vector3 directionRotation = Vector3.zero;
        if (Input.GetKey(KeyCode.Q)) directionRotation = Vector3.up;
        if (Input.GetKey(KeyCode.E)) directionRotation = Vector3.down;

        transform.Rotate(directionRotation, Space.World);
    }
}
