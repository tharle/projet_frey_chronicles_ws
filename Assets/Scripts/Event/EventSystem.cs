using System;
using System.Collections.Generic;

public class EventSystem
{
    private static EventSystem m_Instance;
    public static EventSystem Instance {  
        get {
            if (m_Instance == null) m_Instance = new EventSystem();
            return m_Instance; 
        } 
    }

    private Dictionary<EGameState, Action<bool>> m_Events;

    private EventSystem() 
    {
        m_Events = new Dictionary<EGameState, Action<bool>>();
    }

    public void SubscribeTo(EGameState gameStateId, Action<bool> func)
    {
        if (m_Events.ContainsKey(gameStateId)) m_Events[gameStateId] += func;
        else m_Events.Add(gameStateId, func);
    }

    public void UnsubscribeTo(EGameState gameStateId, Action<bool> func)
    {
        if (m_Events.ContainsKey(gameStateId)) m_Events[gameStateId] -= func;
    }

    public void TriggerEvent(EGameState gameStateId, bool isActive)
    {
        if (m_Events.ContainsKey(gameStateId))
            m_Events[gameStateId]?.Invoke(isActive);
    }
}
