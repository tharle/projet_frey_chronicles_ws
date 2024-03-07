using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class DungeonTargetManager: MonoBehaviour
{

    private static DungeonTargetManager m_Instance;

    public static DungeonTargetManager Instance { 
        get {
            return m_Instance;
        } 
    }

    /// <summary>
    /// Toute les targets de la dugeon
    /// </summary>
    private List<TargetController> m_Targets;

    /// <summary>
    /// Les targets qui sont dans le range du player dans la phase de iterration 
    /// </summary>
    private List<TargetController> m_TargetsInRange;

    private int m_IndexSelected;

    [SerializeField] private EnemyData m_EnemiesData; // Temp
    [SerializeField] private Vector2 m_SpawnPositionRange; // Temp

    private void Awake()
    {
        if (m_Instance != null) Destroy(gameObject);

        m_Instance = this;
    }

    private void Start()
    {
        m_Targets = new List<TargetController>();
        m_TargetsInRange = new List<TargetController>();
        m_IndexSelected = 0;

        LoadAll();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionMode);
    }


    // REFRESH

    private void LoadAll()
    {

        foreach (ITarget target in m_EnemiesData.Enemies)
        {
            TargetController targetController = target is Enemy ? LoadEnemyPrefab((Enemy)target) : LoadThingrefab((Thing)target);
            m_Targets.Add(targetController);
        }
    }

    // TODO changer ça pour un "PoolingPrefabs"
    private TargetController LoadEnemyPrefab(Enemy enemy)
    {

        string urlPrefab = "Enemy/";
        switch (enemy.enemyTypeId)
        {
            case EEnemyType.BAT:
            default:
                urlPrefab += "Enemy";
                break;
        }
        GameObject go = Resources.Load<GameObject>(urlPrefab);
        go.name = enemy.Name;
        go.transform.position = CreateRandomPosition();
        go = Instantiate(go);

        EnemyController enemyController = go.AddComponent<EnemyController>();
        enemyController.Enemy = enemy;

        return enemyController;
    }

    // TODO changer ça pour un "PoolingPrefabs"
    private TargetController LoadThingrefab(Thing thing)
    {
        return null;
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
        m_TargetsInRange = m_Targets.FindAll(target =>
            {
                target.IsSelected = target.IsInPlayerRange();
                return target.IsSelected;
            }
        );

        m_IndexSelected = 0;
        OnSetSelected();
    }

    private void ClearSelectedEnemies()
    {
        Debug.Log("***********************CLEAR******************************");
        m_TargetsInRange.Clear();
    }

    private void OnInterractionMode(bool isEnterState)
    {
        if (isEnterState)
        {
            SelectEnemiesInPlayerRange();
        }
        else
        {
            foreach (EnemyController enemy in m_Targets)
            {
                enemy.IsSelected = false;
            }
            ClearSelectedEnemies();
        }
    }

    private void OnSetSelected()
    {
        TargetController target = m_TargetsInRange.Count != 0 ? m_TargetsInRange[m_IndexSelected] : null;
        SelectSphere.Instance.SelectTarget(target); // pas dde probleme passer null, ça vaut dire quil y a rien pour selectionner
    }

    public void NextSelected()
    {
        if (m_TargetsInRange.Count == 0) return;

        m_IndexSelected++;
        m_IndexSelected %= m_TargetsInRange.Count;

        OnSetSelected();
    }

    public void PreviusSelected()
    {
        if (m_TargetsInRange.Count == 0) return;

        m_IndexSelected--; 
        m_IndexSelected = m_IndexSelected < 0? m_TargetsInRange.Count - 1 : m_IndexSelected;

        OnSetSelected();
    }


}
