using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private const float ANGLE_TO_HIDE_WALLS_RIGHT = -30.0f * Mathf.Deg2Rad;
    private const float ANGLE_TO_HIDE_WALLS_LEFT = 30.0f * Mathf.Deg2Rad;

    private Transform m_LookAtDefault;
    private CinemachineVirtualCamera m_Camera;
    private Transform m_LookAtCurrent;

    private List<Wall> m_WallHiddings = new ();    

    int m_LayerMaskWall;

    private void Start()
    {
        m_Camera = GetComponent<CinemachineVirtualCamera>();
        m_LookAtDefault = m_Camera.LookAt;
        m_LookAtCurrent = m_LookAtDefault;
        m_LayerMaskWall = 1 << LayerMask.NameToLayer(GameParametres.LayerMaskName.WALL);

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

        if (directionRotation.magnitude > 0) 
        {
            transform.Rotate(directionRotation, Space.World);
            ShowWalls();
            HideWalls();
        }
        
    }

    private void ShowWalls() 
    {
        foreach (Wall wall in m_WallHiddings) wall.Show(true);
        m_WallHiddings.Clear();
    }

    private void HideWalls()
    {
        Vector3 catetusForwardRight = -transform.forward;
        catetusForwardRight.x = catetusForwardRight.x * Mathf.Cos(ANGLE_TO_HIDE_WALLS_RIGHT)
                                + catetusForwardRight.z * Mathf.Sin(ANGLE_TO_HIDE_WALLS_RIGHT);
        catetusForwardRight.y = 0;
        catetusForwardRight.z = catetusForwardRight.z * Mathf.Cos(ANGLE_TO_HIDE_WALLS_RIGHT)
                                - catetusForwardRight.x * Mathf.Sin(ANGLE_TO_HIDE_WALLS_RIGHT);
        Debug.DrawRay(transform.position, catetusForwardRight * 50f, Color.green);

        Vector3 catetusForwardLeft = -transform.forward;
        catetusForwardLeft.x = catetusForwardLeft.x * Mathf.Cos(ANGLE_TO_HIDE_WALLS_LEFT)
                                + catetusForwardLeft.z * Mathf.Sin(ANGLE_TO_HIDE_WALLS_LEFT);
        catetusForwardLeft.y = 0;
        catetusForwardLeft.z = catetusForwardLeft.z * Mathf.Cos(ANGLE_TO_HIDE_WALLS_LEFT)
                                - catetusForwardLeft.x * Mathf.Sin(ANGLE_TO_HIDE_WALLS_LEFT);
        Debug.DrawRay(transform.position, catetusForwardLeft * 50f, Color.yellow);

        DetectAndHideAllWalls(catetusForwardRight);
        DetectAndHideAllWalls(catetusForwardLeft);
    }

    private void DetectAndHideAllWalls(Vector3 direction)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, 50f, m_LayerMaskWall);
        foreach (RaycastHit hit in hits)
        {
            Wall wall = hit.collider.gameObject.GetComponent<Wall>();
            if (wall == null) continue;

            wall.Show(false);
            m_WallHiddings.Add(wall);
        }
    }
}
