using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDTargetDisplayInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TargetDamage;
    [SerializeField] protected TextMeshProUGUI m_TargetDescription;

    [SerializeField] private GameObject m_TargetDisplayInfoPanel;

    private void Start()
    {
        SubscribeAllNotifyEvents();
    }

    private void SubscribeAllNotifyEvents()
    {
        //SelectSphere.Instance.OnTargetSelected = NotifyShowSelectTarget;
        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
        GameEventSystem.Instance.SubscribeTo(EGameEvent.SelectTarget, OnTargetSelected);
    }

    private void OnTargetSelected(GameEventMessage message)
    {

        if (message.Contains<ITarget>(EGameEventMessage.Target, out ITarget target))
        {
            m_TargetDamage.text = target != null ? target.DisplayDamage() : "No target in range";
            m_TargetDescription.text = target != null ? target.DisplayDescription() : "";
        }
    }

    private void OnInterractionMode(bool inIntration)
    {
        m_TargetDisplayInfoPanel.SetActive(inIntration);
    }
}
