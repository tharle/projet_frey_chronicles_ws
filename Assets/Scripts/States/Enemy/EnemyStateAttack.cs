using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack : AEnemyState
{

    public EnemyStateAttack(EnemyController controller) : base(EEnemyState.Attack, controller)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        m_Controller.StartCoroutine(AttackRoutine()) ;
    }

    private IEnumerator AttackRoutine()
    {
        m_Controller.EnemyAnimator.SetTrigger(GameParametres.AnimationEnemy.TRIGGER_ATTACK);

        yield return new WaitForSeconds(0.1f);
        while (m_Controller.EnemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(GameParametres.AnimationEnemy.NAME_ATTACK))
        {
            yield return null;
        }

        m_Controller.GiveDamageToPlayer();
    }
}
