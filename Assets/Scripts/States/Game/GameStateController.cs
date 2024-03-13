using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController: MonoBehaviour
{
    private AGameState m_CurrentState;

    private Dictionary<EGameState, AGameState> GameStates;

    //private bool m_IsChangingState();

    private void Start()
    {
        GameStates = new Dictionary<EGameState, AGameState>();
        GameStates.Add(EGameState.None, new NoneState(this));
        GameStates.Add(EGameState.ActionMenu, new ActionMenuState(this));
        GameStates.Add(EGameState.Menu, new MenuState(this));
        GameStates.Add(EGameState.Interaction, new InteractionState(this));
        GameStates.Add(EGameState.Combo, new ComboState(this));
        GameStates.Add(EGameState.Spell, new SpellState(this));

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
        StartCoroutine(ChangeStateRoutine(gameStateId));
    }


    // TODO BUG: if in transition between two diff states, its called two updates in same "Frame"
    // None -> Interraction ->>> open sphere interaction [None] and open Action Menu [Interraction]
    private IEnumerator ChangeStateRoutine(EGameState gameStateId)
    {
        yield return new WaitForSeconds(0.1f);
        m_CurrentState = GameStates[gameStateId];
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
    Spell
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
        GameStateEvent.Instance.Call(m_GameStateId, true);
    }

    public virtual void OnExit() 
    {
        // Trigger le event de "exit state" dans le event system
        GameStateEvent.Instance.Call(m_GameStateId, false);
    }
}

