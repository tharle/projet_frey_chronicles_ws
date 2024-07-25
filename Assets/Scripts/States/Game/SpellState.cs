using System.Collections;
using UnityEngine;

public class SpellState : AGameState
{

    private Spell m_Spell;
    private ATargetController m_Target;

    public SpellState(GameStateController controller) : base(controller, EGameState.Spell)
    {

    }

    public override void OnEnter()
    {
        PlayerAnimation.Instance.SpellCast();

        m_Message.Contains<ATargetController>(EGameEventMessage.TargetController, out m_Target);
        m_Message.Contains<Spell>(EGameEventMessage.Spell, out m_Spell);

        m_Controller.StartCoroutine(DoSpellRoutine());
        base.OnEnter();
    }

    private IEnumerator DoSpellRoutine()
    {
        // TODO add animation for spell
        //GameEventSystem.Instance.TriggerEvent(EGameEvent.CastMagic, m_Message);

        Effect effectTrace = EffectPoolManager.Instance.Get(m_Spell.EffectTrace);
        effectTrace.LifeTimeInSeconds = 2.5f;
        effectTrace.FowardToScreen = 0;
        effectTrace.transform.localScale = Vector3.one * 5f;

        if (!m_Spell.IsJustTarget)
        {
            float timeForWait = effectTrace.LifeTimeInSeconds / 100f;
            effectTrace.LifeTimeInSeconds = 0;
            effectTrace.DoEffect(m_Controller.transform);
            for (int i = 0; i <= 100; i++)
            {
                effectTrace.transform.position = Vector3.Lerp(m_Controller.transform.position, m_Target.transform.position, 0.01f * ((float)i));
                yield return new WaitForSeconds(timeForWait);
            }
            effectTrace.DestroyIt();
        }
        else
        {
            effectTrace.DoEffect(m_Target.transform);
        }

        Effect effectCollition = EffectPoolManager.Instance.Get(m_Spell.EffectCollision);
        effectCollition.LifeTimeInSeconds = 2.5f;
        effectCollition.FowardToScreen = 0;
        effectCollition.transform.localScale = Vector3.one * 10f;
        effectCollition.DoEffect(m_Target.transform);

        yield return new WaitForSeconds(effectCollition.LifeTimeInSeconds + 0.01f);

        m_Target.ReciveSpell(m_Spell);
        PlayerController.Instance.ConsumeTension(m_Spell.TensionCost);
        yield return new WaitForSeconds(1f);
        m_Controller.ChangeState(EGameState.None);
    }
}
