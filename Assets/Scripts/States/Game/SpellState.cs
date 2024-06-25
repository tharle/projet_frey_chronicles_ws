using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SpellState : AGameState
{
    private ATargetController m_Target; // Last target selected in Sphere
    public SpellState(GameStateController controller) : base(controller, EGameState.Spell)
    {

        GameEventSystem.Instance.SubscribeTo(EGameEvent.SelectTarget, OnSelectTarget);
    }

    private void OnSelectTarget(GameEventMessage message)
    {
        if(message.Contains<ATargetController>(EGameEventMessage.TargetController, out ATargetController target) && target != null)
        {
            m_Target = target;
        }
    }

    public override void OnEnter()
    {
        m_Controller.StartCoroutine(DoSpellRoutine());
        base.OnEnter();
    }

    private IEnumerator DoSpellRoutine()
    {
        // TODO add animation for spell
        GameEventMessage message = new GameEventMessage(EGameEventMessage.Spell, PlayerController.Instance.FireBall);
        message.Add(EGameEventMessage.TargetController, m_Target);
        GameEventSystem.Instance.TriggerEvent(EGameEvent.CastMagic, message);
        yield return new WaitForSeconds(0.5f);

        m_Controller.ChangeState(EGameState.None);
    }
}
