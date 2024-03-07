﻿using UnityEngine;

public class InteractionState : AGameState
{
    public InteractionState(GameStateController controller) : base(controller, EGameState.Interaction)
    {
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Change to NONE
            m_Controller.ChangeState(EGameState.None);
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
