using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyStateAttack : AEnemyState
{
    public EnemyStateAttack(EnemyController controller) : base(EEnemyState.Attack, controller)
    {
    }

    public override void UpdateState()
    {
        DoAttack();
    }

    // Juste pour montrer l'idee du turn des ennemies
    private void DoAttack()
    {
        // TODO: Add NavMesh pour chaque Enemy
        Transform enemyTransform = m_Controller.gameObject.transform;
        Vector3 direction = PlayerController.Instance.GetDirectionTo(enemyTransform.position);
        enemyTransform.forward = direction;
        enemyTransform.Translate(direction * m_Controller.Enemy.SpeedMovement * Time.deltaTime); // TODO: Change for Physics
    }
}
