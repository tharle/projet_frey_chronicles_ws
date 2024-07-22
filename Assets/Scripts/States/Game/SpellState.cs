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
    private List<Rune> m_Runes;

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
