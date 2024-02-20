using UnityEngine;

public class InteractionState : IGameState
{

    public void OnEnter(GameStateController stateController)
    {
        // Ouvre la sphere d'interaction
        //m_GameController.OnInterationMode(true);
        EventSystem.GetInstance().TriggerEvent(EGameState.Interaction, true);
    }

    public void OnExit(GameStateController stateController)
    {
        // Ferme la sphere d'iteraction
        //m_GameController.OnInterationMode(false);
        EventSystem.GetInstance().TriggerEvent(EGameState.Interaction, false);
    }

    public void UpdateState(GameStateController stateController)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Change to NONE
            stateController.ChangeState(EGameState.None);
        }
    }
}
