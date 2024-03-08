using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HUDGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_HPValue;
    [SerializeField] TextMeshProUGUI m_TPValue;
    [SerializeField] TextMeshProUGUI m_APValue;

    private void Awake()
    {
        SubscribeAllNotifyEvents();
    }

    private void SubscribeAllNotifyEvents()
    {
        PlayerController.Instance.OnNotifyInfoPlayer += OnNotifyInfoPlayer;
    }

    private void OnNotifyInfoPlayer(Player player)
    {
        m_HPValue.text = ((player.HitPoints / player.HitPointsMax) * 100).ToString("00");
        m_TPValue.text = ((player.TensionPoints / player.TensionPointsMax) * 100).ToString("00");
        m_APValue.text = ((player.ActionPoints / player.ActionPointsMax) * 100).ToString("00");
    }
}
