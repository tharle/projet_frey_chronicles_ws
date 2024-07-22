using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CastSpellState : AGameState
{
    private ATargetController m_Target; // Last target selected in Sphere

    private List<ERune> m_CastedRunes;
    private List<Rune> m_Runes;

    public CastSpellState(GameStateController controller) : base(controller, EGameState.CastSpell)
    {

        GameEventSystem.Instance.SubscribeTo(EGameEvent.SelectTarget, OnSelectTarget);
        LoadAllRunes();
    }

    private void LoadAllRunes()
    {
        List<RuneData> runes = BundleLoader.Instance.LoadAll<RuneData, ERune>(GameParametres.BundleNames.RUNES);
        m_Runes = new();
        foreach (var data in runes) m_Runes.Add(data.Value);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_CastedRunes = new List<ERune>();
    }


    private void OnSelectTarget(GameEventMessage message)
    {
        if (message.Contains<ATargetController>(EGameEventMessage.TargetController, out ATargetController target) && target != null)
        {
            m_Target = target;
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();

        foreach (Rune rune in m_Runes)
        {
            foreach (KeyCode keyCode in rune.KCodes)
                if (Input.GetKeyDown(keyCode)) CastRune(rune);
        }

        if(m_CastedRunes.Count >= 6) m_Controller.ChangeState(EGameState.Spell);
    }

    private void CastRune(Rune rune)
    {
        m_CastedRunes.Add(rune.Type);
        GameEventSystem.Instance.TriggerEvent(EGameEvent.AddRunes, new GameEventMessage(EGameEventMessage.Rune, rune));
    }
}
