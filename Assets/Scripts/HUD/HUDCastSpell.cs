using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class HUDCastSpell : MonoBehaviour
{
    [SerializeField] Transform m_RunesContainer;
    [SerializeField] GameObject m_SpellCastBoard;

    [SerializeField] GameObject m_ResultSpell;
    [SerializeField] GameObject m_SpellFail;
    [SerializeField] GameObject m_SpellSucess;
    [SerializeField] TextMeshProUGUI m_SpellName;
    [SerializeField] TextMeshProUGUI m_SpellFailDescription;



    void Start()
    {
        m_ResultSpell.SetActive(false);
        m_SpellCastBoard.SetActive(false);
        SubscribeAll();
    }

    private void OnDestroy()
    {
        UnsubscribeAll();
    }

    private void SubscribeAll()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.AddRunes, OnAddRunes);
        GameStateEvent.Instance.SubscribeTo(EGameState.CastSpell, OnCastSpellState);
        GameEventSystem.Instance.SubscribeTo(EGameEvent.ResultSpell, OnResultSpell);
    }

    private void OnAddRunes(GameEventMessage message)
    {
        m_RunesContainer.gameObject.SetActive(true);
        if (message.Contains<Rune>(EGameEventMessage.Rune, out Rune rune))
        {
            GameObject go = new GameObject(Enum.GetName(typeof(ERune), rune.Type));
            go.transform.position = Vector3.zero;
            go.transform.SetParent(m_RunesContainer, false);
            Image image = go.AddComponent<Image>();
            image.sprite = rune.Icon;
        }
    }

    private void UnsubscribeAll()
    {
        GameEventSystem.Instance.UnsubscribeFrom(EGameEvent.AddRunes, OnAddRunes);
        GameStateEvent.Instance.UnsubscribeFrom(EGameState.CastSpell, OnCastSpellState);
    }

    private void OnCastSpellState(bool isEnter)
    {
        m_SpellCastBoard.SetActive(isEnter);
        m_ResultSpell.SetActive(!isEnter);
        if (isEnter) CleanBoard();

        // TODO change camera(?)
    }

    private void CleanBoard()
    {
        foreach (Transform runeCasted in m_RunesContainer) Destroy(runeCasted.gameObject);
    }

    private void OnResultSpell(GameEventMessage message)
    {
        m_RunesContainer.gameObject.SetActive(false);
        m_ResultSpell.SetActive(true);
        m_SpellFail.SetActive(true);
        m_SpellSucess.SetActive(false);
        if (!message.Contains<string>(EGameEventMessage.SpellName, out string spellName))
        {
            // Spell unknown
            m_SpellName.text = "?????";
            m_SpellFailDescription.text = "You can't cast a spell you don't know!";
            return;
        }

        m_SpellName.text = spellName;
        if (message.Contains<int>(EGameEventMessage.TensionCost, out int tensionPoints))
        {
            m_SpellFailDescription.text = $"You need {tensionPoints} TP for that spell!";
            return;
        }

        // Sucess
        m_SpellFail.SetActive(false);
        m_SpellSucess.SetActive(true);
        m_SpellFailDescription.text = "";

    }
}
