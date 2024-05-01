using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class EnemyController : ATargetController
{
    private Enemy m_Enemy;
    public Enemy Enemy { set { m_Enemy = value; } get { return m_Enemy; } }
    private NavMeshAgent m_NavMeshAgent;
    

    private bool m_Running;

    private AEnemyState m_CurrentState;
    private Dictionary<EEnemyState, AEnemyState> m_States;

    protected override void AfterAwake() // Meme que le Start()
    {
        base.AfterAwake();
        SubscribeAll();
        LoadStates();
        SetUpEnemy();
    }

    private void SetUpEnemy()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_NavMeshAgent.speed = m_Enemy.SpeedMovement;
        m_NavMeshAgent.acceleration = m_Enemy.SpeedMovement * 2; // pour qu'il puisse arriver dans speed max dans 0.5s
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
        if (m_CurrentState is EnemyStateMove && collision.collider.CompareTag(GameParametres.TagName.PLAYER))
        {
            EnemyTurnManager.Instance.Next();
        }
    }

    private void LoadStates()
    {
        m_States = new Dictionary<EEnemyState, AEnemyState>();
        m_States.Add(EEnemyState.Move, new EnemyStateMove(this));
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
    /// Change le destination du NavMeshAgent de l'Enemy
    /// </summary>
    /// <param name="destination"></param>
    public void MoveTo(Vector3 destination)
    {
        m_NavMeshAgent.destination = destination;
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
        if (m_CurrentState is EnemyStateMove) EnemyTurnManager.Instance.Next();
    }

}
