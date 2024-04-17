using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionState : AGameState
{
    public InteractionState(GameStateController controller) : base(controller, EGameState.Interaction)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Change to NONE
            m_Controller.ChangeState(EGameState.None);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // TODO: Enter in "choisir entre magie et combo"
            //PlayerController.Instance.AttackSelected();
            m_Controller.ChangeState(EGameState.ActionMenu);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DungeonTargetManager.Instance.NextSelected();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            DungeonTargetManager.Instance.PreviusSelected();
        }
    }
}
