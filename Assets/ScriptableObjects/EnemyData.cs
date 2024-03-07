using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITarget
{
    public string DisplayDamage();
    public string DisplayDescription();
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
    public float HitPoints;
    public float HitPointsMax;
    public float TensionPoints;
    public float SpeedMovement;
    public int SpeedInitiative;
    public float Evasion; // 0 - 1
    public EEnemyType enemyTypeId;

    public string DisplayDamage()
    {
        return $"{Name} : {enemyTypeId} {Evasion * 100}%";
    }

    public string DisplayDescription()
    {
        return $"Resist: Air | Weakeness: Water";
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
