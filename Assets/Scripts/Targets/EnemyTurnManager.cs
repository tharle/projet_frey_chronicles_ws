using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    private List<EnemyController> m_enemies;
    private int m_currentIndex;
    private EnemyController m_enemyCurrent;

    private static EnemyTurnManager m_instance;
    public static EnemyTurnManager Instance
    {
        get {
            return m_instance; 
        }
    }

    private void Awake()
    {
        if(m_instance != null) Destroy(gameObject);
        m_instance = this;
    }

    public void LoadAllEnemies()
    {
        m_enemies = DungeonTargetManager.Instance.GetAllBy<EnemyController>();

        m_enemies = m_enemies.OrderByDescending(enemy => enemy.Enemy.SpeedInitiative).ToList();
        m_currentIndex = 0;
        if (m_enemies.Count > 0)
        {
            m_enemyCurrent = m_enemies[m_currentIndex];
            m_enemyCurrent.ChangeState(EEnemyState.Move);
        }
    }

    public void Next()
    {
        m_enemyCurrent?.ChangeState(EEnemyState.Wait);
        
        if (m_enemies.Count <= 0) return;

        m_enemyCurrent = GetNextEnemyAvable();
        m_enemyCurrent?.ChangeState(EEnemyState.Move);

        Debug.Log($"IS TURN OF: {m_currentIndex}");
    }

    private EnemyController GetNextEnemyAvable() 
    {
        if (m_enemies.Count <= 0) return null;

        m_currentIndex++;
        m_currentIndex %= m_enemies.Count;
        EnemyController enemy = m_enemies[m_currentIndex];

        if (!enemy.IsDestroyed() && enemy.IsAlive()) return enemy;

        m_enemies.Remove(enemy);
        m_currentIndex--;

        return GetNextEnemyAvable();
    }
}
