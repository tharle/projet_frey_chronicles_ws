using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController: MonoBehaviour
{
    private GameController m_GameController;

    private IGameState m_CurrentState;

    private Dictionary<EGameState, IGameState> GameStates;

    private void Start()
    {
        GameStates = new Dictionary<EGameState, IGameState>();
        m_GameController = GetComponent<GameController>();

        GameStates.Add(EGameState.None, new NoneState());
        GameStates.Add(EGameState.Menu, new MenuState());
        GameStates.Add(EGameState.Interaction, new InteractionState());
        GameStates.Add(EGameState.Combo, new ComboState());
        GameStates.Add(EGameState.Spell, new SpellState());

        ChangeState(EGameState.None);
    }

    void Update()
    {
        if (m_CurrentState != null)  m_CurrentState.UpdateState(this);
    }

    public void ChangeState(EGameState newState) 
    {
        if(m_CurrentState != null)  m_CurrentState.OnExit(this);
        m_CurrentState = GameStates[newState];
        m_CurrentState.OnEnter(this);
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

public interface IGameState
{
    public void OnEnter(GameStateController stateController);
    public void UpdateState(GameStateController stateController);
    public void OnExit(GameStateController stateController);
}

