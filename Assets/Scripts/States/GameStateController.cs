using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController: MonoBehaviour
{
    private AGameState m_CurrentState;

    private Dictionary<EGameState, AGameState> GameStates;

    private void Start()
    {
        GameStates = new Dictionary<EGameState, AGameState>();
        GameStates.Add(EGameState.None, new NoneState(this));
        GameStates.Add(EGameState.Menu, new MenuState(this));
        GameStates.Add(EGameState.Interaction, new InteractionState(this));
        GameStates.Add(EGameState.Combo, new ComboState(this));
        GameStates.Add(EGameState.Spell, new SpellState(this));

        ChangeState(EGameState.None);
    }

    void Update()
    {
        if (m_CurrentState != null)  m_CurrentState.UpdateState();
    }

    public void ChangeState(EGameState newState) 
    {
        if(m_CurrentState != null)  m_CurrentState.OnExit();
        m_CurrentState = GameStates[newState];
        m_CurrentState.OnEnter();
    }
}



public enum EGameState
{
    None        = 0,
    Combo       = 1,
    Interaction = 2,
    Menu        = 4,
    Spell       = 8
}

public abstract class AGameState
{
    protected EGameState m_GameStateId;
    protected GameStateController m_Controller;

    public AGameState(GameStateController controller,  EGameState gameStateId)
    {
        m_Controller = controller;
        m_GameStateId = gameStateId;
    }
    public abstract void UpdateState();

    public virtual void OnEnter()
    {
        // Trigger le event de "enter state" dans le event system
        EventSystem.GetInstance().TriggerEvent(m_GameStateId, true);
    }

    public virtual void OnExit() 
    {
        // Trigger le event de "exit state" dans le event system
        EventSystem.GetInstance().TriggerEvent(m_GameStateId, false);
    }
}

