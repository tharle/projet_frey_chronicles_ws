using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_Doors;

    [SerializeField]
    private bool m_Acessible = true;

    [SerializeField] private List<EnemyData> m_Enemies; // Temp
    [SerializeField] private Transform m_SpawnPointMin;
    [SerializeField] private Transform m_SpawnPointMax;

    [SerializeField] private List<ThingData> m_Things;

    private void Start()
    {
        if(m_Acessible) DungeonRoomManager.Instance.AddRoom(this);
    }

    public void Enter()
    {
        LoadAll();
    }

    private void LoadAll()
    {
        List<ATargetController> targets = new List<ATargetController>();
        foreach (EnemyData enemy in m_Enemies)
        {
            ATargetController targetController = LoadAndInstantieteEnemy(enemy.Value);
            targets.Add(targetController);
        }

        foreach (ThingData thing in m_Things)
        {
            ATargetController targetController = LoadAndInstantieteThing(thing.Value);
            targets.Add(targetController);
        }

        GameEventSystem.Instance.TriggerEvent(EGameEvent.LoadTargets, new GameEventMessage(EGameEventMessage.Targets, targets));
    }

    // TODO changer ça pour un "PoolingPrefabs"
    private ATargetController LoadAndInstantieteEnemy(Enemy enemy)
    {
        GameObject go = EnemyPoolManager.Instance.Get(enemy.TypeId);
        go.name = enemy.Name;
        go.SetActive(true);
        go.transform.position = CreateRandomPosition();
        go.transform.SetParent(transform);

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
        Vector3 positionRangeMin = m_SpawnPointMin.transform.position;
        Vector3 positionRangeMax = m_SpawnPointMax.transform.position;
        position.x = Random.Range(positionRangeMin.x, positionRangeMax.x);
        position.y = 1;
        position.z = Random.Range(positionRangeMin.z, positionRangeMax.z);

        return position;
    }

    // TODO: faire prendre la door d'une façon logique
    public Vector3 GetRandomDoor()
    {
        int randPos = Random.Range(0, m_Doors.Count);

        return m_Doors[randPos].position;
    }

}
