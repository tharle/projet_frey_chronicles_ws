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
    public SpellState(GameStateController controller) : base(controller, EGameState.Spell)
    {
    }

    public override void OnEnter()
    {
        m_Controller.StartCoroutine(DoSpellRoutine());
        base.OnEnter();
    }

    private IEnumerator DoSpellRoutine()
    {
        // TODO add animation for spell
        GameEventSystem.Instance.TriggerEvent(EGameEvent.CastMagic, new GameEventMessage(EGameEventMessage.Spell, PlayerController.Instance.FireBall));
        yield return new WaitForSeconds(0.5f);

        m_Controller.ChangeState(EGameState.None);
    }
}
