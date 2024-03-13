using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class EnemyController : ATargetController
{
    private Enemy m_Enemy;
    public Enemy Enemy { set { m_Enemy = value; } }

    private bool m_Running;

    protected override void AfterStart() // Meme que le Start()
    {
        base.AfterStart();
        GameStateEvent.Instance.SubscribeTo(EGameState.None, OnNoneState);
    }

    private void OnDestroy()
    {
        GameStateEvent.Instance.UnsubscribeTo(EGameState.None, OnNoneState);
    }
    private void Update()
    {
        if (m_Running) DoActionEnemy();
    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log($"COLITION with {collision.collider.tag}");
        // TODO: Temporaire pour changer le 
        if (m_Enemy.StateId == EEnemyState.Attack && collision.collider.CompareTag(GameParametres.TagName.PLAYER))
        {
            EnemyTurnManager.Instance.Next();
        }
    }

    private void OnNoneState(bool isEnterState)
    {
        m_Running = isEnterState;
    }


    private void DoActionEnemy()
    {
        switch (m_Enemy.StateId)
        {
            case EEnemyState.Attack:
                DoAttack();
                break; 
            case EEnemyState.Wait:
                // DoWait();
                break;
        }
    }

    private void DoWait()
    {
        // TODO: Add NavMesh pour chaque Enemy
    }

    // Juste pour montrer l'idee du turn des ennemies
    private void DoAttack()
    {
        // TODO: Add NavMesh pour chaque Enemy
        Vector3 direction = PlayerController.Instance.GetDirectionTo(transform.position);
        transform.forward = direction;
        transform.Translate(direction * m_Enemy.SpeedMovement * Time.deltaTime); // TODO: Change for Physics
    }

    public bool IsAlive()
    {
        return m_Enemy.HitPoints > 0;
    }

    public override ITarget GetTarget()
    {
        return m_Enemy;
    }

    public void SetState(EEnemyState enemyStateId)
    {
        m_Enemy.StateId = enemyStateId;
    }

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
            TargetDie();
        }else
        {
            Debug.Log($"HP {m_Enemy.HitPoints}/{m_Enemy.HitPointsMax}");
        }


        return m_Enemy.TensionPoints;
    }

    public override int ReciveAttack(int value)
    {
        Debug.Log($"The player give {value} damage to {m_Enemy.Name}.");
        m_Enemy.HitPoints -= value;
        
        if (!IsAlive()) TargetDie();

        return m_Enemy.TensionPoints;
    }

    public override void ReciveSpell(int value, EElemental elementalId)
    {
        m_Enemy.HitPoints -= value;

        if (!IsAlive()) TargetDie();
    }

    public int GetIniciative()
    {
        return m_Enemy.SpeedInitiative;
    }

    protected override void TargetDie()
    {
        base.TargetDie();
        if (m_Enemy.StateId == EEnemyState.Attack) EnemyTurnManager.Instance.Next();
    }

}
