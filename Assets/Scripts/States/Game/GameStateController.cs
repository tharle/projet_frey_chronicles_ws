using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController: MonoBehaviour
{
    private AGameState m_CurrentState;

    private Dictionary<EGameState, AGameState> m_States;

    private PlayerMovement m_PlayerMovement;
    public PlayerMovement Movement {  get { return m_PlayerMovement; } }

    //private bool m_IsChangingState();

    private void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();

        m_States = new Dictionary<EGameState, AGameState>();
        m_States.Add(EGameState.None, new NoneState(this));
        m_States.Add(EGameState.ActionMenu, new ActionMenuState(this));
        m_States.Add(EGameState.Menu, new MenuState(this));
        m_States.Add(EGameState.Interaction, new InteractionState(this));
        m_States.Add(EGameState.Combo, new ComboState(this));
        m_States.Add(EGameState.Spell, new SpellState(this));
        m_States.Add(EGameState.Touch, new TouchState(this));

        ChangeState(EGameState.None);
    }

    void Update()
    {
        if (m_CurrentState != null) m_CurrentState.UpdateState();
    }

    public void ChangeState(EGameState gameStateId) 
    {
     //   Debug.Log($"CHANGE STATE FROM {m_CurrentState?.GetState()} --> {gameStateId}");
        m_CurrentState?.OnExit();
        m_CurrentState = m_States[gameStateId];
        m_CurrentState?.OnEnter();
        //StartCoroutine(ChangeStateRoutine(gameStateId));
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_CurrentState?.OnCollisionEnter(collision);
    }


    // TODO BUG: if in transition between two diff states, its called two updates in same "Frame"
    // None -> Interraction ->>> open sphere interaction [None] and open Action Menu [Interraction]
    private IEnumerator ChangeStateRoutine(EGameState gameStateId)
    {
        yield return new WaitForSeconds(0.1f);
        m_CurrentState = m_States[gameStateId];
        m_CurrentState?.OnEnter();
    }

}



public enum EGameState
{
    None,
    ActionMenu,
    Combo,
    Interaction,
    Menu,
    Spell,
    Touch
}

public abstract class AGameState
{
    protected EGameState m_GameStateId;
    protected GameStateController m_Controller;

    public EGameState GetState() { return m_GameStateId; }

    public AGameState(GameStateController controller,  EGameState gameStateId)
    {
        m_Controller = controller;
        m_GameStateId = gameStateId;
    }
    public virtual void UpdateState(){}

    public virtual void OnEnter()
    {
        // Trigger le event de "enter state" dans le event system
        m_Controller.Movement.StopMove();
        GameStateEvent.Instance.Call(m_GameStateId, true);
    }

    public virtual void OnExit() 
    {
        // Trigger le event de "exit state" dans le event system
        GameStateEvent.Instance.Call(m_GameStateId, false);
    }

    public virtual void OnCollisionEnter(Collision collision){}
}

