using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EElemental
{
    Air,
    Fire,
    Water
}

[Serializable]
public struct Player : ITarget
{
    public string Name;
    public int HitPoints;
    public int HitPointsMax;
    public int TensionPoints;
    public int TensionPointsMax;
    public float ActionPoints;
    public int ActionPointsMax;
    public int ActionPointPerSec;
    public float DistanceAttack;
    public float RefreshTime;
    public Vector2 DamageRange; // TODO: Temp, il faut changer ça dans le systeme de combat

    public Player(int hitPointsMax, float distanceAttack)
    {
        Name = "Player";
        HitPointsMax = hitPointsMax;
        HitPoints = HitPointsMax;
        DistanceAttack = distanceAttack;
        TensionPoints = 0;
        TensionPointsMax = 100;
        ActionPoints = 0;
        ActionPointsMax = 100;
        ActionPointPerSec = 45;
        RefreshTime = 0.1f;
        DamageRange = new Vector2(3, 15);
    }

    public float GetHPRatio()
    {
        return (float)HitPoints / (float)HitPointsMax;
    }

    public float GetAPRatio()
    {
        return (float)ActionPoints / (float)ActionPointsMax;
    }

    public float GetTPRatio()
    {
        return (float)TensionPoints / (float)TensionPointsMax;
    }

    public int GetDamage()
    {
        return UnityEngine.Random.Range(Mathf.FloorToInt(DamageRange.x), Mathf.FloorToInt(DamageRange.y));
    }

    public void AddActionPoints()
    {
        ActionPoints += ActionPointPerSec * RefreshTime;
        ActionPoints = ActionPoints > ActionPointsMax ? ActionPointsMax : ActionPoints;
    }

    public bool IsAction()
    {
        return ActionPoints >= ActionPointsMax;
    }

    public void ConsumeActionPoints()
    {
        ActionPoints = 0;
    }

    public string DisplayDamage()
    {
        return $"{Name} {HitPoints}/{HitPointsMax}";
    }

    public string DisplayDescription()
    {
        return "Self";
    }

    public bool IsAlive()
    {
        return HitPoints > 0;
    }

    public float GetRange()
    {
        return DistanceAttack;
    }
}
