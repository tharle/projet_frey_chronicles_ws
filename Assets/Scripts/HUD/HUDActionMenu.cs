using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class HUDActionMenu : MonoBehaviour
{

    [SerializeField] private GameObject m_ActionMenuPanel;
    [SerializeField] private Button m_BtnAttack;
    private void Awake()
    {
        SubscribeAllNotifyEvents();
    }

    private void Start()
    {
        m_ActionMenuPanel.SetActive(false);
    }

    private void SubscribeAllNotifyEvents()
    {
        GameStateEvent.Instance.SubscribeTo(EGameState.ActionMenu, OnActionMenu);
        //SelectSphere.Instance.OnTargetSelected += OnTargetSelected;
        GameEventSystem.Instance.SubscribeTo(EGameEvent.SelectTarget, OnTargetSelected);
    }

    private void OnActionMenu(bool isEnterState)
    {
        m_ActionMenuPanel.SetActive(isEnterState);
    }

    private void OnTargetSelected(GameEventMessage message)
    {

        if (message.Contains<ITarget>(EGameEventMessage.Target, out ITarget target))
        {
            // TODO: Changer pour une plus belle view
            m_BtnAttack.gameObject.SetActive(target is not Player); 
        }
    }

    public void OnClickAttack()
    {
        PlayerController.Instance.AttackSelected();
    }

    public void OnClickSpell()
    {
        PlayerController.Instance.SpellSelected();
    }

}
