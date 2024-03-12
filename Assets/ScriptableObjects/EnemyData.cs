using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyType
{
    Bat
}

[Serializable]
public struct Enemy : ITarget
{
    public int id;
    public string Name;
    public int HitPoints;
    public int HitPointsMax;
    public int TensionPoints;
    public float SpeedMovement;
    public int SpeedInitiative;
    public EEnemyType enemyTypeId;
    public EElemental elementalId;

    public string DisplayDamage()
    {
        return $"{Name} {HitPoints}/{HitPointsMax}";
    }

    public string DisplayDescription()
    {
        return $"Type: {enemyTypeId} - {elementalId}";
    }
}

public enum EThingType
{
    BAT
}
[Serializable]
public struct Thing : ITarget
{
    public int id;
    public string Name;

    public string DisplayDamage()
    {
        return "DISPLAY  DAMAGE THING";
    }

    public string DisplayDescription()
    {
        return "DISPLAY  DESCRIPTION THING";
    }
}

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [SerializeField] public List<Enemy> Enemies;
    [SerializeField] public List<Thing> Things;
}
