using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EEnemyState
{
    Wait,
    Attack
}

public abstract class AEnemyState
{
    protected EEnemyState m_EnemyStateId;
    protected EnemyController m_Controller;

    public AEnemyState(EEnemyState enemyStateId, EnemyController controller)
    {
        m_EnemyStateId = enemyStateId;
        m_Controller = controller;
    }

    public virtual void UpdateState() {}
}
