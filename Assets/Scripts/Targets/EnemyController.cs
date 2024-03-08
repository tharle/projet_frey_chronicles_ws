using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : TargetController
{
    private Enemy m_Enemy;
    public Enemy Enemy { set { m_Enemy = value; } }

    /// <summary>
    /// Take damage and return the amount of Tension for the player
    /// </summary>
    /// <param name="damage">the damage of the enemy suffer</param>
    /// <returns>amount of Tension for the player</returns>
    public int TakeDamage(int damage)
    {
        m_Enemy.HitPoints -= damage;

        Debug.Log($"The enemy {m_Enemy.Name} got {damage} the damage.");
        if (m_Enemy.HitPoints <= 0) 
        {
            Debug.Log($"He DIE!");
            Destroy(gameObject);
        }else
        {
            Debug.Log($"HP {m_Enemy.HitPoints}/{m_Enemy.HitPointsMax}");
        }


        return m_Enemy.TensionPoints;
    }

    public bool IsDead()
    {
        return m_Enemy.HitPoints <= 0;
    }

    public override ITarget GetTarget()
    {
        return m_Enemy;
    }
}
