using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITarget
{
}

public enum EEnemyType
{
    BAT
}

[Serializable]
public struct Enemy : ITarget
{
    public int id;
    public string Name;
    public int SpeedMovement;
    public int SpeedInitiative;
    public EEnemyType enemyTypeId;
}

public enum EThingType
{
    BAT
}
[Serializable]
public struct  Thing: ITarget
{
    public int id;
    public string Name;
}

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [SerializeField] public List<Enemy> Enemies;
    [SerializeField] public List<Thing> Things;
}
