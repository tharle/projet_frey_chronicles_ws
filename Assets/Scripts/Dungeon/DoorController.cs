using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(GameParametres.TagName.PLAYER))
        {
            DungeonRoomManager.Instance.GoToNextRoom();
        }
    }
}
