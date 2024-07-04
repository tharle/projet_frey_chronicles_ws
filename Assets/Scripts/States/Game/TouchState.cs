using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchState : AGameState
{
    public TouchState(GameStateController controller) : base(controller, EGameState.Touch)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerAnimation.Instance.Touch();
        m_Controller.StartCoroutine(WaitToFinishAnimation());
    }

    IEnumerator WaitToFinishAnimation()
    {
        yield return new WaitForSeconds(1.5f);

        m_Controller.ChangeState(EGameState.None);
    }
}
