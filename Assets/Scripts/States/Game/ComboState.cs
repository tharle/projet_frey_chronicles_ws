using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class ComboState : AGameState
{
    public ComboState(GameStateController controller) : base(controller, EGameState.Combo)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();

        m_Controller.ChangeState(EGameState.None);
    }
}
