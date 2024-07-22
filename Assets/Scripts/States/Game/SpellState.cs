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

        GameEventSystem.Instance.TriggerEvent(EGameEvent.CastMagic, m_Message);
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
