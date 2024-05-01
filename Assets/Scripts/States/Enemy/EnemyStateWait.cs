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
        Debug.Log($"Enemy {m_Controller.name} is waiting.");
    }

}
