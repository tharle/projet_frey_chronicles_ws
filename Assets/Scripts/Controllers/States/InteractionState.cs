using UnityEngine;

public class InteractionState : AGameState
{
    public InteractionState(GameStateController stateController, GameController gameController) : base(stateController, gameController)
    {
    }

    public override void OnEnter()
    {
        // Ouvre la sphere d'interaction
        m_GameController.OnInterationMode(true);
    }

    public override void OnExit()
    {
        // Ferme la sphere d'iteraction
        m_GameController.OnInterationMode(false);
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Change to NONE
            m_StateController.ChangeState(EGameState.None);
        }
    }
}
