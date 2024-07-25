using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class CastSpellState : AGameState
{
    private ATargetController m_Target; // Last target selected in Sphere

    private List<ERune> m_CastedRunes;
    private List<Rune> m_Runes;

    private Effect m_PreparingEffect;

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
        PlayerAnimation.Instance.SpellPreparing();
        m_CastedRunes = new List<ERune>();

        PlayerController.Instance.LookToTarget(m_Target);

        m_PreparingEffect = EffectPoolManager.Instance.Get(EEffect.Preparing);
        m_PreparingEffect.DoEffect(m_Controller.transform);
    }

    public override void OnExit()
    {
        base.OnExit();
        m_PreparingEffect.DestroyIt(0.01f);
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

            foreach (MouseButton mouseButton in rune.MButtons) 
                if (Input.GetMouseButtonDown((int) mouseButton)) CastRune(rune);
        }

        if (m_CastedRunes.Count >= 6)
        {
            if (PlayerController.Instance.GetSpell(m_CastedRunes, out Spell spell))
            {
                NotifyResult(spell);
            } else
            {
                NotifyResult(null);
            }
        }
    }

    private void CastRune(Rune rune)
    {
        m_CastedRunes.Add(rune.Type);
        GameEventSystem.Instance.TriggerEvent(EGameEvent.AddRunes, new GameEventMessage(EGameEventMessage.Rune, rune));
        
        if (PlayerController.Instance.GetSpell(m_CastedRunes, out Spell spell))
        {
            NotifyResult(spell);
        }
    }

    private void NotifyResult(Spell spell)
    {
        bool sucess = PlayerController.Instance.HasPlayerTPForSpell(spell);

        GameEventMessage message = new GameEventMessage();
        if (spell != null) message.Add(EGameEventMessage.SpellName, spell.Name);
        if (spell != null && !sucess) message.Add(EGameEventMessage.TensionCost, spell.TensionCost);

        GameEventSystem.Instance.TriggerEvent(EGameEvent.ResultSpell, message);

        m_Controller.StartCoroutine(WaitAndCloseRoutine(sucess, spell));
    }

    private IEnumerator WaitAndCloseRoutine(bool sucess, Spell spell)
    {
        yield return new WaitForSeconds(3f);

        if (sucess) DoCastSpell(spell);
        else m_Controller.ChangeState(EGameState.None);
    }

    private void DoCastSpell(Spell spell)
    {
        GameEventMessage message = new GameEventMessage(EGameEventMessage.Spell, spell);
        message.Add(EGameEventMessage.TargetController, m_Target);
        m_Controller.ChangeState(EGameState.Spell, message);
    }

}
