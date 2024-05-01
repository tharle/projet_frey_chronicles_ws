using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class EnemyController : ATargetController
{
    private Enemy m_Enemy;
    public Enemy Enemy { set { m_Enemy = value; } get { return m_Enemy; } }

    private bool m_Running;

    private AEnemyState m_CurrentState;
    private Dictionary<EEnemyState, AEnemyState> m_States;

    protected override void AfterAwake() // Meme que le Start()
    {
        base.AfterAwake();
        SubscribeAll();

        LoadStates();        
    }
    private void OnDestroy()
    {
        UnsubscribeAll();
    }

    private void Update()
    {
        if (m_Running) m_CurrentState.UpdateState();
    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log($"COLITION with {collision.collider.tag}");
        // TODO: Temporaire pour changer le 
        if (m_CurrentState is EnemyStateAttack && collision.collider.CompareTag(GameParametres.TagName.PLAYER))
        {
            EnemyTurnManager.Instance.Next();
        }
    }

    private void LoadStates()
    {
        m_States = new Dictionary<EEnemyState, AEnemyState>();
        m_States.Add(EEnemyState.Attack, new EnemyStateAttack(this));
        m_States.Add(EEnemyState.Wait, new EnemyStateWait(this));

        ChangeState(EEnemyState.Wait);
    }

    private void SubscribeAll()
    {
        GameStateEvent.Instance.SubscribeTo(EGameState.None, OnNoneState);
    }
    private void UnsubscribeAll()
    {
        GameStateEvent.Instance.UnsubscribeFrom(EGameState.None, OnNoneState);
    }

    private void OnNoneState(bool isEnterState)
    {
        m_Running = isEnterState;
    }

    public bool IsAlive()
    {
        return m_Enemy.HitPoints > 0;
    }

    public override ITarget GetTarget()
    {
        return m_Enemy;
    }

    public void ChangeState(EEnemyState stateId)
    {
        m_CurrentState = m_States[stateId];
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

    protected override void TargetDie()
    {
        base.TargetDie();
        if (m_CurrentState is EnemyStateAttack) EnemyTurnManager.Instance.Next();
    }

}
