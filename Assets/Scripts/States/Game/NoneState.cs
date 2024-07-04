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
        PlayerAnimation.Instance?.ResetAnimation();
        base.OnEnter();
        m_Controller.Movement.StartMoving();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (Input.GetKeyDown(KeyCode.Space)) OpenIntractionSphere();

        m_Controller.Movement.Move();
    }

    private void OpenIntractionSphere()
    {
        if (PlayerController.Instance.IsAction())
        {
            // Change to interaction
            m_Controller.ChangeState(EGameState.Interaction);
        }
        else
        {
            Debug.Log("NO ACTION AVAIBLE. WAIT!");
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.collider.TryGetComponent<DoorController>(out DoorController doorController))
        {
            doorController.OpenDoor();
            m_Controller.ChangeState(EGameState.Touch);
        }
    }
}
