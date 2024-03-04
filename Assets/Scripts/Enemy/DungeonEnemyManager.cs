using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonEnemyManager: MonoBehaviour
{

    private static DungeonEnemyManager m_Instance;

    public static DungeonEnemyManager Instance { 
        get {
            if (m_Instance == null) m_Instance = new DungeonEnemyManager();

            return m_Instance;
        } 
    }

    /// <summary>
    /// Toute les Enemies de la dugeon
    /// </summary>
    private List<EnemyController> m_Enemies;

    /// <summary>
    /// Les ennemies qui sont dans le range du player dans la phase de iterration 
    /// </summary>
    private List<EnemyController> m_EnemiesInRange;

    [SerializeField] private EnemyData m_EnemiesData; // Temp
    [SerializeField] private Vector2 m_SpawnPositionRange; // Temp

    private void Start()
    {
        LoadEnemies();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        EventSystem.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
    }


    // REFRESH

    private void LoadEnemies()
    {
        m_Enemies = new List<EnemyController>();
        m_EnemiesInRange = new List<EnemyController>();

        foreach (EnemyData.Enemy enemy in m_EnemiesData.EnemyList)
        {
            GameObject go = LoadEnemyPrefab(enemy.enemyTypeId);
            go.transform.position = CreateRandomPosition();
            go = Instantiate(go);
            go.name = enemy.Name;

            EnemyController enemyController = go.AddComponent<EnemyController>();
            enemyController.Enemy = enemy;
            m_Enemies.Add(enemyController);
        }
    }

    // TODO changer ça pour un "PoolingPrefabs"
    private GameObject LoadEnemyPrefab(EEnemyType enemyTypeId)
    {
        string urlPrefab = "Enemy/";
        switch (enemyTypeId)
        {
            case EEnemyType.BAT:
            default:
                urlPrefab += "Enemy";
                break;
        }
        return Resources.Load<GameObject>(urlPrefab);
    }


    // TODO : Change pour un "List of spots" de la scene;
    private Vector3 CreateRandomPosition()
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(m_SpawnPositionRange.x, m_SpawnPositionRange.y);
        position.y = 1;
        position.z = Random.Range(m_SpawnPositionRange.x, m_SpawnPositionRange.y);

        return position;
    }

    private void SelectEnemiesInPlayerRange()
    {
        Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        m_EnemiesInRange = m_Enemies.FindAll(enemy =>
            {
                // Change color for in range to Blue
                Debug.Log("---------------------------------------------");
                Debug.Log(enemy.name);
                enemy.IsSelected = enemy.IsInPlayerRange();
                Debug.Log(enemy.IsSelected);


                return enemy.IsSelected;
            }
        );
    }

    private void ClearSelectedEnemies()
    {
        Debug.Log("***********************CLEAR******************************");
        m_EnemiesInRange.Clear();
    }

    private void OnInterractionMode(bool isEnterState)
    {
        if (isEnterState)
        {
            SelectEnemiesInPlayerRange();
        }
        else
        {
            foreach (EnemyController enemy in m_Enemies)
            {
                enemy.IsSelected = false;
            }
            ClearSelectedEnemies();
        }

    }


}
