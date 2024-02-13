using UnityEngine;

public class GameStateController: MonoBehaviour
{
    private GameController m_GameController;

    private AGameState m_CurrentState;

    public NoneState m_NoneState;
    public MenuState m_MenuState;
    public InteractionState m_InteractionState;
    public ComboState m_ComboState;
    public SpellState m_SpellState;

    private void Start()
    {
        m_GameController = GetComponent<GameController>();
        m_NoneState = new NoneState(this, m_GameController);
        m_MenuState = new MenuState(this, m_GameController);
        m_InteractionState = new InteractionState(this, m_GameController);
        m_ComboState = new ComboState(this, m_GameController);
        m_SpellState = new SpellState(this, m_GameController);

        ChangeState(m_NoneState);
    }

    void Update()
    {
        if (m_CurrentState != null)  m_CurrentState.UpdateState();
    }

    public void ChangeState(AGameState newState) 
    {
        if(m_CurrentState != null)  m_CurrentState.OnExit();
        m_CurrentState = newState;
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

