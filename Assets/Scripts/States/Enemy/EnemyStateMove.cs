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

        MoveToNaveMesh();

        // DoAttackNavMesh();
    }

    // Juste pour montrer l'idee du turn des ennemies
    private void MoveTo()
    {
        // TODO: Add NavMesh pour chaque Enemy
        Transform enemyTransform = m_Controller.gameObject.transform;
        Vector3 direction = PlayerController.Instance.GetDirectionTo(enemyTransform.position);
        enemyTransform.forward = direction;
        enemyTransform.Translate(direction * m_Controller.Enemy.SpeedMovement * Time.deltaTime); // TODO: Change for Physics
    }

    private void MoveToNaveMesh()
    {
        // TODO: Calculer la distance
        //m_NavMeshAgent.Move();
        m_Controller.MoveTo(PlayerController.Instance.transform.position);
    }

}
