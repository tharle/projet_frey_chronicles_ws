using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyType
{
    Bat
}

public enum EEnemyState
{
    Wait,
    Attack
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
    public float DistanceAttack;
    public EEnemyType TypeId;
    public EEnemyState StateId;
    public EElemental ElementalId;

    public string DisplayDamage()
    {
        return $"{Name} {HitPoints}/{HitPointsMax}";
    }

    public string DisplayDescription()
    {
        return $"Type: {TypeId} - {ElementalId}";
    }

    public bool IsAlive()
    {
        return HitPoints > 0;
    }

    public float GetSpeedMovimentWait()
    {
        return SpeedMovement/2;
    }

    public float GetRange()
    {
        return DistanceAttack;
    }
}

public enum EThingType
{
    DESTRUTIBLE,
    INDESTRUTIBLE
}
[Serializable]
public struct Thing : ITarget
{
    public int id;
    public string Name;
    public int HitPoints;

    public string DisplayDamage()
    {
        return "DISPLAY  DAMAGE THING";
    }

    public string DisplayDescription()
    {
        return "DISPLAY  DESCRIPTION THING";
    }

    public bool IsAlive()
    {
        return HitPoints > 0;
    }

    public float GetRange()
    {
        return 0;
    }
}

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [SerializeField] public List<Enemy> Enemies;
    [SerializeField] public List<Thing> Things;
}
