using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class ComboState : AGameState
{
    public ComboState(GameStateController stateController, GameController gameController) : base(stateController, gameController)
    {
    }

    public override void OnEnter()
    {
        // Ouvre la sphere d'interaction
    }

    public override void OnExit()
    {
        // Ferme la sphere d'iteraction

    }

    public override void UpdateState()
    {
        // ??
    }
}
