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
        effectCollition.transform.localScale = Vector3.one * 20f;
        effectCollition.DoEffect(m_Target.transform);

        yield return new WaitForSeconds(effectCollition.LifeTimeInSeconds + 0.01f);

        m_Target.ReciveSpell(m_Spell);
        yield return new WaitForSeconds(2f);
        m_Controller.ChangeState(EGameState.None);
    }


    //TODO use that for magic
    /*    private IEnumerator CastComboRoutine()
        {
            // Animation
            PlayerAnimation.Instance.Attack();

            PlayerController.Instance.LookToTarget(m_Target);

            yield return new WaitForSeconds(0.3f);

            //Logic
            GameObject go = BundleLoader.Instance.Load<GameObject>(GameParametres.BundleNames.PREFAB_COMBO, "Attack");
            go.transform.position = PlayerController.Instance.PlayerHand.position;

            if(go.TryGetComponent<ProjectilCombo>(out ProjectilCombo projectil))
            {
                projectil.Lauch(m_Target, 5f, OnHit);
            }
        }

        private void OnHit(ATargetController target)
        {
            AudioManager.Instance.Play(EAudio.Attack, m_Controller.transform.position);
            GameEventSystem.Instance.TriggerEvent(EGameEvent.ComboDamageToEnemy, new GameEventMessage(EGameEventMessage.TargetController, target));

            if(!m_AttackWasPressed) m_CurrentRoutine = m_Controller.StartCoroutine(DoComboRoutine());
            else m_Controller.ChangeState(EGameState.None);
        }*/
}
