using UnityEngine;

public class GameStateController: MonoBehaviour
{
    private IGameState m_CurrentState;

    public NoneState m_NoneState = new NoneState();
    public MenuState MenuState = new MenuState();
    public InteractionState m_InteractionState = new InteractionState();
    public ComboState m_ComboState = new ComboState();
    public SpellState m_SpellState = new SpellState();

    private void Start()
    {
        ChangeState(m_NoneState);
    }

    void Update()
    {
        if (m_CurrentState != null)  m_CurrentState.UpdateState(this);
    }

    public void ChangeState(IGameState newState) 
    {
        if(m_CurrentState != null)  m_CurrentState.OnExit(this);
        m_CurrentState = newState;
        m_CurrentState.OnEnter(this);
    }
}
public interface IGameState
{
    public void OnEnter(GameStateController controller);
    public void UpdateState(GameStateController controller);
    public void OnExit(GameStateController controller);
}

