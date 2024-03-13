using UnityEngine;

public class InterractController : MonoBehaviour
{

    private void Start()
    {
        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
    }

    private void OnInterractionMode(bool isEnterState)
    {

    }
}
