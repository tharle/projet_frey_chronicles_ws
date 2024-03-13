using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ActionMenuState : AGameState
{
    public ActionMenuState(GameStateController controller) : base(controller, EGameState.ActionMenu)
    {
    }

    public override void OnEnter()
    {
        PlayerController.Instance.OnAttackSelected += OnClickAttack;
        PlayerController.Instance.OnSpellSelected += OnClickSpell;

        base.OnEnter();
    }

    public override void OnExit()
    {
        PlayerController.Instance.OnAttackSelected -= OnClickAttack;
        PlayerController.Instance.OnSpellSelected -= OnClickSpell;

        base.OnExit();

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Change to NONE
            m_Controller.ChangeState(EGameState.Interaction);
        }
    }


    private void OnClickAttack()
    {
        m_Controller.ChangeState(EGameState.Combo);
    }

    private void OnClickSpell()
    {
        m_Controller.ChangeState(EGameState.Spell);
    }

}
