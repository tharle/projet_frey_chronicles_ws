using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDComboDisplayInfo : MonoBehaviour
{
    [SerializeField] private GameObject m_ComboTimerIcon;
    [SerializeField] private TextMeshProUGUI m_ComboValue;
    [SerializeField] private GameObject m_ComboInfoPanel;
    [SerializeField] private float m_Duration = 1.0f;

    private float m_ElapseTime;

    private void Awake()
    {
        SubscribeAllNotifyEvents();
    }

    private void SubscribeAllNotifyEvents()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.ComboInfoHUD, OnComboInfoHud);
        GameEventSystem.Instance.SubscribeTo(EGameEvent.ComboTimerToggle, OnComboTimerToggle);
    }

    private void OnComboInfoHud(GameEventMessage message)
    {
        if (message.Contains<int>(EGameEventMessage.ComboValue, out int comboValue))
        {
            if (comboValue < 0) 
            {
                StartCoroutine(HideComboDisplay());
                return;
            }

            m_ComboValue.text = comboValue.ToString();
            m_ComboInfoPanel.SetActive(true);
        }
    }

    private void OnComboTimerToggle(GameEventMessage message)
    {
        if (message.Contains<bool>(EGameEventMessage.ComboTimerToggle, out bool show))
        {
            m_ComboTimerIcon.SetActive(show);
        }
    }

    private IEnumerator HideComboDisplay()
    {
        yield return new WaitForSeconds(m_Duration);
        m_ComboInfoPanel.SetActive(false);
    }
}
