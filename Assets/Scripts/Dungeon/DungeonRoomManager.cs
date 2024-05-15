using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonRoomManager : MonoBehaviour
{
    private static DungeonRoomManager m_Instance;

    public static DungeonRoomManager Instance { get { return m_Instance; } }

    private List<RoomController> m_Rooms = new List<RoomController>();
    private int m_IndexCourrentRoom;

    private void Awake()
    {
        if (m_Instance != null) Destroy(m_Instance);

        m_Instance = this;
    }

    private void Start()
    {
        m_IndexCourrentRoom = 1;
    }

    public void AddRoom(RoomController room)
    {
        m_Rooms.Add(room);
    }

    public void GoToNextRoom()
    {

        if (DungeonTargetManager.Instance.GetAllBy<EnemyController>().Count > 0) 
        {
            Debug.Log("You can exit without kill all enemies.");
            return;
        } 

        // TODO: ajouter des logics pour faire du sense le changement des rooms
        m_IndexCourrentRoom++;
        m_IndexCourrentRoom%=m_Rooms.Count;
        RoomController room = m_Rooms[m_IndexCourrentRoom];
        room.Enter();

        GameEventSystem.Instance.TriggerEvent(EGameEvent.EnterRoom, new GameEventMessage(EGameEventMessage.Room, room));
    }
}
