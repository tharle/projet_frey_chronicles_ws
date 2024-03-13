using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_Doors;

    private void Start()
    {
        DungeonRoomManager.Instance.AddRoom(this);
    }

    // TODO: faire prendre la door d'une façon logique
    public Vector3 GetRandomDoor()
    {
        int randPos = Random.Range(0, m_Doors.Count);

        return m_Doors[randPos].position;
    }
}
