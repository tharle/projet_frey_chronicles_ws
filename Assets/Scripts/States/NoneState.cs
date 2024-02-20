using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NoneState : AGameState
{
    public NoneState(GameStateController controller) : base(controller, EGameState.None)
    {
    }

    public override void OnEnter()
    {
        Time.timeScale = 1.0f;
        base.OnEnter();
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Change to interaction
            m_Controller.ChangeState(EGameState.Interaction);
        }
    }
}
