using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class SpellState : AGameState
{
    public SpellState(GameStateController controller) : base(controller, EGameState.Spell)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();

        m_Controller.ChangeState(EGameState.None);
    }
}
