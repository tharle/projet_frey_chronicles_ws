using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class EnemyController : ATargetController
{
    private Enemy m_Enemy;
    public Enemy Enemy { get => m_Enemy;
        set 
        {
            m_Enemy = value;
            SetUpEnemy();
        } 
    }
    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    public Animator EnemyAnimator => m_Animator;
    

    private bool m_Running;

    private AEnemyState m_CurrentState;
    private Dictionary<EEnemyState, AEnemyState> m_States;


    private void OnDestroy()
    {
        UnsubscribeAll();
    }

    private void Update()
    {
        if (m_Running)
        {
            m_Animator?.SetFloat(GameParametres.AnimationEnemy.FLOAT_VELOCITY, m_NavMeshAgent.velocity.magnitude);
            m_CurrentState?.UpdateState();
        }
    }

    protected override void AfterAwake() // Meme que le Start()
    {
        base.AfterAwake();
        SubscribeAll();
        LoadStates();
    }

    protected override void AfterStart() {
        m_Animator = GetComponentInChildren<Animator>();
        base.AfterStart();
    }

    private void LoadStates()
    {
        m_States = new Dictionary<EEnemyState, AEnemyState>();
        m_States.Add(EEnemyState.Move, new EnemyStateMove(this));
        m_States.Add(EEnemyState.Wait, new EnemyStateWait(this));
        m_States.Add(EEnemyState.Attack, new EnemyStateAttack(this));

        ChangeState(EEnemyState.Wait);
    }

    private void SetUpEnemy()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_NavMeshAgent.speed = m_Enemy.SpeedMovement/2;
        m_NavMeshAgent.acceleration = m_Enemy.SpeedMovement; // pour qu'il puisse arriver dans speed max dans 1s
        m_NavMeshAgent.stoppingDistance = m_Enemy.DistanceAttack;
        m_Running = true;
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
        m_NavMeshAgent.isStopped = !isEnterState;
    }

    public override ITarget GetTarget()
    {
        return m_Enemy;
    }

    public void ChangeState(EEnemyState stateId)
    {
        m_CurrentState?.ExitState();
        m_CurrentState = m_States[stateId];
        m_CurrentState.EnterState();
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
    public override int ReciveAttack(int value)
    {
        Debug.Log($"The player give {value} damage to {m_Enemy.Name}.");
        m_Animator.SetTrigger(GameParametres.AnimationEnemy.TRIGGER_HIT);
        m_Enemy.HitPoints -= value;
        
        if (!IsAlive()) TargetDie();

        return m_Enemy.TensionPoints;
    }

    public override void ReciveSpell(Spell spell)
    {
        m_Animator.SetTrigger(GameParametres.AnimationEnemy.TRIGGER_HIT);

        // TODO calc speel force
        m_Enemy.HitPoints -= (int) spell.BaseDamage;

        if (!IsAlive()) TargetDie();
    }

    protected override void TargetDie()
    {
        base.TargetDie();
        m_Animator.SetTrigger(GameParametres.AnimationEnemy.TRIGGER_DIE);
        if (m_CurrentState is EnemyStateMove) GoNextEnemy();
    }

    public bool IsPlayerInAttackRange()
    {
        return IsInRange(PlayerController.Instance.transform.position);
    }

    public bool IsPlayerInFleeRange()
    {
        float distance = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        return distance <= m_Enemy.DistanceFlee;
    }

    public Vector3 DirectionFromPlayer()
    {
        return (transform.position - PlayerController.Instance.transform.position).normalized;
    }

    public void StopMove()
    {
        m_NavMeshAgent.ResetPath();
    }


    public void GiveDamageToPlayer()
    {
        // TODO: give damage to player
        Debug.Log($"GIVE DAMAGE TO PLAYER: {name} / {m_Enemy.Name} -> {m_Enemy.DamageAttack}");
        GameEventMessage message = new GameEventMessage(EGameEventMessage.DamageAttack, m_Enemy.DamageAttack);
        message.Add(EGameEventMessage.DamageElemental, m_Enemy.ElementalId);
        GameEventSystem.Instance.TriggerEvent(EGameEvent.DamageToPlayer, message);
        GoNextEnemy();
    }

    public void GoNextEnemy()
    {
        EnemyTurnManager.Instance.Next();
    }
}
