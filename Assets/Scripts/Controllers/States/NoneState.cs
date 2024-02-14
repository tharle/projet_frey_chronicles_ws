using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NoneState : AGameState
{
    public NoneState(GameStateController stateController, GameController gameController) : base(stateController, gameController)
    {
    }

    public override void OnEnter()
    {
        Time.timeScale = 1.0f;
    }

    public override void OnExit()
    {

    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Change to interaction
            m_StateController.ChangeState(EGameState.Interaction);
        }
    }
}
