using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpellState : AGameState
{
    public CastSpellState(GameStateController controller, EGameState gameStateId) : base(controller, EGameState.CastSpell)
    {
    }
}
