using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct HudBarData
{
    public float CurrentValue;
    public float MaxValue;

    public float Ratio => CurrentValue / MaxValue;
    public int Percentual => Mathf.FloorToInt(Ratio * 100);
}

public class HUDBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextValue;
    [SerializeField] private Slider m_SliderValue;
    [SerializeField] private bool m_IsPercentual;
    [SerializeField] private EGameEvent m_UpdateEventId;



    private void Start()
    {
        SubscribeAll();
    }

    private void SubscribeAll()
    {
        GameEventSystem.Instance.SubscribeTo(m_UpdateEventId, OnUpdateEvent);
    }

    private void OnUpdateEvent(GameEventMessage message)
    {
        if(message.Contains<HudBarData>(EGameEventMessage.HudBarData, out HudBarData hudBarData))
        {
            RefeshUI(hudBarData);
            RefeshText(hudBarData);
        }
    }

    private void RefeshText(HudBarData hudBarData)
    {
        if (m_IsPercentual) m_TextValue.text = hudBarData.Percentual + "%";
        else m_TextValue.text = Mathf.FloorToInt(hudBarData.CurrentValue) + "/" + Mathf.FloorToInt(hudBarData.MaxValue); 
    }

    private void RefeshUI(HudBarData hudBarData)
    {
        m_SliderValue.value = hudBarData.Ratio;
    }
}
