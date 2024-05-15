using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateWait : AEnemyState
{
    public EnemyStateWait(EnemyController controller) : base(EEnemyState.Wait, controller)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (m_Controller.IsPlayerInFleeRange())
        {
            FleeFromPlayer();
            return;
        }


        m_Controller.StopMove();

    }

    private void FleeFromPlayer()
    {
        Vector3 direction = m_Controller.DirectionFromPlayer();
        m_Controller.MoveTo(direction * 10);
    }
}
