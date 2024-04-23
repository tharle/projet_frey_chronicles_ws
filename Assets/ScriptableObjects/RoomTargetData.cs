using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoomTargetData : ScriptableObject
{
    [SerializeField] public List<Enemy> Enemies;
    [SerializeField] public List<Thing> Things;
}
