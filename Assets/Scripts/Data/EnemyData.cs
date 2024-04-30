using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyType
{
    Bat,
    Wolf,
    Zombie
}

public enum EEnemyState
{
    Wait,
    Attack
}

[Serializable]
public struct Enemy : ITarget
{
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
        return SpeedMovement; // TODO: changer pour une velocite plus lente
    }

    public float GetRange()
    {
        return DistanceAttack;
    }
}

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public Enemy Value;
}
