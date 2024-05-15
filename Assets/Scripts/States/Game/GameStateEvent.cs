using System;
using System.Collections.Generic;

public class GameStateEvent
{
    private Dictionary<EGameState, Action<bool>> m_Events;


    private static GameStateEvent m_Instance;
    public static GameStateEvent Instance {  
        get {
            if (m_Instance == null) m_Instance = new GameStateEvent();
            return m_Instance; 
        } 
    }


    private GameStateEvent() 
    {
        m_Events = new Dictionary<EGameState, Action<bool>>();
    }

    public void SubscribeTo(EGameState gameStateId, Action<bool> func)
    {
        if (m_Events.ContainsKey(gameStateId)) m_Events[gameStateId] += func;
        else m_Events.Add(gameStateId, func);
    }

    public void UnsubscribeFrom(EGameState gameStateId, Action<bool> func)
    {
        if (m_Events.ContainsKey(gameStateId)) m_Events[gameStateId] -= func;
    }

    public void Call(EGameState gameStateId, bool isEnter)
    {
        if (m_Events.ContainsKey(gameStateId))
            m_Events[gameStateId]?.Invoke(isEnter);
    }
}
