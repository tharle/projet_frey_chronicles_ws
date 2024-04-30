using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameEventMessage
{
    Targets,
    Room
}

public class GameEventMessage
{
    private Dictionary<EGameEventMessage, object> m_Params;

    public GameEventMessage()
    {
        m_Params = new Dictionary<EGameEventMessage, object>();
    }

    public GameEventMessage(EGameEventMessage messageId, object value) : this()
    {
        Add(messageId, value);
    }


    public void Add(EGameEventMessage eventMessageId, object value)
    {
        if (m_Params.ContainsKey(eventMessageId))
        {
            m_Params[eventMessageId] = value;
        }
        else
        {
            m_Params.Add(eventMessageId, value);
        }
    }

    public bool Contains<T>(EGameEventMessage eventMessageId, out T value)
    {
        value = default;
        if (m_Params.ContainsKey(eventMessageId) && m_Params[eventMessageId] is T)
        {
            value = (T)m_Params[eventMessageId];
            return true;
        }

        return false;
    }
}