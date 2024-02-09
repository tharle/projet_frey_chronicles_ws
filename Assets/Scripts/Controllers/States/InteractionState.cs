using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class InteractionState : IGameState
{
    public void OnEnter(GameStateController controller)
    {
        // Ouvre la sphere d'interaction
    }

    public void OnExit(GameStateController controller)
    {
        // Ferme la sphere d'iteraction

    }

    public void UpdateState(GameStateController controller)
    {
        // ??
    }
}
