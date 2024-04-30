using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour 
{
    #region Singleton
    private static EnemyPoolManager m_Instance;
    public static EnemyPoolManager Instance
    {
        get
        {
            if (m_Instance == null) 
            {
                GameObject go = new GameObject("Enemy Pool Manager");
                go.AddComponent<EnemyPoolManager>();
            }

            return m_Instance;
        }
    }
    #endregion

    private Dictionary<EEnemyType, GameObject> m_Enemies = new Dictionary<EEnemyType, GameObject>();

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
        }

        m_Instance = this;
    }

    public GameObject Get(EEnemyType enemyTypeId) 
    {
        if (!m_Enemies.ContainsKey(enemyTypeId))
        {
            GameObject go = BundleLoader.Instance.Load<GameObject>(GameParametres.BundleNames.PREFAB_ENEMY, Enum.GetName(typeof(EEnemyType), enemyTypeId));
            go.SetActive(false);
            m_Enemies[enemyTypeId] = go;
        }

        return Instantiate(m_Enemies[enemyTypeId]);
    }

}
