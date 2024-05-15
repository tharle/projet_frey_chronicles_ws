using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class EnemyStateMove : AEnemyState
{
    

    public EnemyStateMove(EnemyController controller) : base(EEnemyState.Move, controller)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if(m_Controller.IsPlayerInAttackRange())
        {
            m_Controller.ChangeState(EEnemyState.Attack);
            return;
        }

        MoveToPlayerr();
    }

    private void MoveToPlayerr()
    {
        m_Controller.MoveTo(PlayerController.Instance.transform.position);
    }

}
