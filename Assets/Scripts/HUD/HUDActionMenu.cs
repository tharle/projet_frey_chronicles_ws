using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDActionMenu : MonoBehaviour
{

    [SerializeField] private GameObject m_ActionMenuPanel;
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
    }

    private void OnActionMenu(bool isEnterState)
    {
        m_ActionMenuPanel.SetActive(isEnterState);
    }

    public void OnClickAttack()
    {
        //Change state to Combo
        PlayerController.Instance.AttackSelected();
    }

    public void OnClickSpell()
    {
        // TODO : Change state to Spell
        PlayerController.Instance.SpellSelected();
    }
}
