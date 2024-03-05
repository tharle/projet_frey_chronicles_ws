using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_HPValue;
    [SerializeField] TextMeshProUGUI m_TPValue;

    // Start is called before the first frame update
    void Start()
    {
        SubscribeAllNotifyEvents();
    }

    private void SubscribeAllNotifyEvents()
    {
        PlayerController.Instance.OnHitPoint += NotifyHitPoint;
        PlayerController.Instance.OnTensionPoint += NotifyTensionPoints;
    }

    private void NotifyHitPoint(float hpRatio)
    {
        m_HPValue.text = (hpRatio * 100).ToString("00");
    }

    private void NotifyTensionPoints(float tensionRatio)
    {
        m_TPValue.text = (tensionRatio * 100).ToString("00");
    }
}
