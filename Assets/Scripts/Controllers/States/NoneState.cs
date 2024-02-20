using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NoneState : IGameState
{

    public void OnEnter(GameStateController stateController)
    {
        Time.timeScale = 1.0f;
    }

    public void OnExit(GameStateController stateController)
    {

    }

    public void UpdateState(GameStateController stateController)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Change to interaction
            stateController.ChangeState(EGameState.Interaction);
        }
    }
}
