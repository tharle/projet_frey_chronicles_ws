using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class HUDCastSpell : MonoBehaviour
{
    [SerializeField] Transform m_RunesContainer;
    [SerializeField] GameObject m_SpellCastBoard;
    


    void Start()
    {
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
        GameStateEvent.Instance.SubscribeTo(EGameState.CastSpell, OnEnterState);
    }

    private void OnAddRunes(GameEventMessage message)
    {
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
        GameStateEvent.Instance.UnsubscribeFrom(EGameState.CastSpell, OnEnterState);
    }

    private void OnEnterState(bool isEnter)
    {
        m_SpellCastBoard.SetActive(isEnter);
        if (isEnter) CleanBoard();

        // TODO change camera(?)
    }


    private void CleanBoard()
    {
        foreach (Transform runeCasted in m_RunesContainer.transform) Destroy(runeCasted.gameObject);
    }
}
