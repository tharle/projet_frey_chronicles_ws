using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack : AEnemyState
{
    private Animator m_Animator;

    public EnemyStateAttack(EnemyController controller) : base(EEnemyState.Attack, controller)
    {
        m_Animator = controller.gameObject.GetComponentInChildren<Animator>();
    }

    public override void EnterState()
    {
        base.EnterState();

        m_Controller.StartCoroutine(AttackRoutine()) ;
    }

    private IEnumerator AttackRoutine()
    {
        m_Animator?.SetTrigger(GameParametres.AnimationEnemy.TRIGGER_ATTACK);
        yield return new WaitForSeconds(1f);
        while (m_Animator.GetCurrentAnimatorStateInfo(0).IsName(GameParametres.AnimationEnemy.NAME_ATTACK))
        {
            yield return null;
        }

        m_Controller.GiveDamageToPlayer();
    }
}
