using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private List<ATargetController> m_Targets;

    /// <summary>
    /// Les targets qui sont dans le range du player dans la phase de iterration 
    /// </summary>
    private List<ATargetController> m_TargetsInRange;

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
        m_Targets = new List<ATargetController>();
        m_TargetsInRange = new List<ATargetController>();
        m_IndexSelected = 0;

        LoadAll();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameStateEvent.Instance.SubscribeTo(EGameState.Interaction, OnInterractionState);
        GameStateEvent.Instance.SubscribeTo(EGameState.None, OnNoneState);
    }

    private void OnInterractionState(bool isEnterState)
    {
        if (isEnterState) SelectTargetsInPlayerRange();
    }

    private void OnNoneState(bool isEnterState)
    {
        if (isEnterState) ClearAllSelectedTargets();
    }

    private void ClearAllSelectedTargets()
    {
        foreach (ATargetController target in m_Targets)
        {
            target.IsSelected = false;
        }
        m_TargetsInRange.Clear();
    }


    // REFRESH

    private void LoadAll()
    {

        foreach (Enemy enemy in m_EnemiesData.Enemies)
        {
            ATargetController targetController = LoadAndInstantieteEnemy(enemy);
            m_Targets.Add(targetController);
        }

        foreach (Thing thing in m_EnemiesData.Things)
        {
            ATargetController targetController = LoadAndInstantieteThing(thing);
            m_Targets.Add(targetController);
        }

        m_Targets.Add(PlayerController.Instance);



    }

    // TODO changer ça pour un "PoolingPrefabs"
    private ATargetController LoadAndInstantieteEnemy(Enemy enemy)
    {

        string urlPrefab = "Enemy/";
        switch (enemy.enemyTypeId)
        {
            case EEnemyType.Bat:
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
    private ATargetController LoadAndInstantieteThing(Thing thing)
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

    private void SelectTargetsInPlayerRange()
    {
        // Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        m_TargetsInRange = m_Targets.FindAll(target =>
            {
                target.IsSelected = PlayerController.Instance.IsInRange(target.transform.position);
                return target.IsSelected;
            }
        );

        if (!SelectSphere.Instance.IsTargetSelected()) 
        {
            m_IndexSelected = 0;
            SelectTarget();
        } else
        {
            SelectSphere.Instance.UpdateSelectTarget();
        }
    }

    private void SelectTarget()
    {
        ATargetController target = m_TargetsInRange.Count != 0 ? m_TargetsInRange[m_IndexSelected] : null;
        SelectSphere.Instance.SelectTarget(target); // pas dde probleme passer null, ça vaut dire quil y a rien pour selectionner
    }

    private void OnAttack(int damage)
    {
        if(m_TargetsInRange.Count == 0) {
            Debug.Log("NO TARGETS AVAIBLES");
            return;
        }

        ATargetController target = m_TargetsInRange[m_IndexSelected];

        if (target is EnemyController)
        {
            EnemyController enemy = (EnemyController)target;
            int tension = enemy.TakeDamage(damage);
            PlayerController.Instance.AddTension(tension);

            

        }

        if (target.IsDestroyed()) TargetSelectedDestroyed();

    }

    private void TargetSelectedDestroyed()
    {
        m_Targets.Remove(m_TargetsInRange[m_IndexSelected]);// remove from targets
        m_TargetsInRange.RemoveAt(m_IndexSelected); // remove from targets in range
        NextSelected();
    }

    // -------------------------------------------
    // INPUTS METHODES
    // -------------------------------------------

    public void NextSelected()
    {
        if (m_TargetsInRange.Count == 0) return;

        m_IndexSelected++;
        m_IndexSelected %= m_TargetsInRange.Count;

        SelectTarget();
    }

    public void PreviusSelected()
    {
        if (m_TargetsInRange.Count == 0) return;

        m_IndexSelected--; 
        m_IndexSelected = m_IndexSelected < 0? m_TargetsInRange.Count - 1 : m_IndexSelected;

        SelectTarget();
    }

    public void TargetDie(ATargetController target)
    {
        m_Targets.Remove(target);
        m_TargetsInRange.Remove(target);
    }
}
