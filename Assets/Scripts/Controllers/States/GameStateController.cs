using System.Collections.Generic;
using UnityEngine;

public class GameStateController: MonoBehaviour
{
    private GameController m_GameController;

    private AGameState m_CurrentState;

    private Dictionary<EGameState, AGameState> GameStates;

    private void Start()
    {
        GameStates = new Dictionary<EGameState, AGameState>();
        m_GameController = GetComponent<GameController>();

        GameStates.Add(EGameState.None, new NoneState(this, m_GameController));
        GameStates.Add(EGameState.Menu, new MenuState(this, m_GameController));
        GameStates.Add(EGameState.Interaction, new InteractionState(this, m_GameController));
        GameStates.Add(EGameState.Combo, new ComboState(this, m_GameController));
        GameStates.Add(EGameState.Spell, new SpellState(this, m_GameController));

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
    protected EGameState m_GameState;
    protected GameStateController m_StateController;
    protected GameController m_GameController;

    public AGameState(GameStateController stateController, GameController gameController) 
    {
        m_StateController = stateController; 
        m_GameController = gameController;
    }

    public abstract void OnEnter();
    public abstract void UpdateState();
    public abstract void OnExit();
}

