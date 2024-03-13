using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private Light m_Light;
    // Start is called before the first frame update
    void Start()
    {
        m_Light = GetComponent<Light>();

        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
        GameStateEvent.Instance.SubscribeTo(EGameState.None, OnNoneMode);
    }

    private void OnNoneMode(bool isEnterState)
    {
        if (isEnterState) ChangeLight(true);
    }

    private void OnInterractionMode(bool isEnterState)
    {
       if(isEnterState) ChangeLight(false);
    }

    private void ChangeLight(bool turnLightOn) 
    { 
        m_Light.enabled = turnLightOn;
    }
}
