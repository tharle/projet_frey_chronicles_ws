using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class MenuState : AGameState
{
    public MenuState(GameStateController controller) : base(controller, EGameState.Menu)
    {
    }

    public override void UpdateState()
    {
        //throw new NotImplementedException();
    }
}
