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
    public float HitPoints;
    public float HitPointsMax;
    public int TensionPoints;
    public int TensionPointsMax;
    public float ActionPoints;
    public int ActionPointsMax;
    public int ActionPointPerSec;
    public float DistanceAttack;
    public float RefreshTime;
    public Vector2 DamageRange; // TODO: Temp, il faut changer ça dans le systeme de combat
    public List<Spell> SpellTome;

    public Player(float hitPointsMax, float distanceAttack)
    {
        Name = "Player";
        HitPointsMax = hitPointsMax;
        HitPoints = HitPointsMax;
        DistanceAttack = distanceAttack;
        TensionPoints = 0;
        TensionPointsMax = 100;
        ActionPoints = 0;
        ActionPointsMax = 100;
        ActionPointPerSec = 75;
        RefreshTime = 0.1f;
        DamageRange = new Vector2(3, 15);
        SpellTome = new();
    }

    public HudBarData GetHPData()
    {
        HudBarData hpData;
        hpData.CurrentValue = HitPoints;
        hpData.MaxValue = HitPointsMax;

        return hpData;
    }

    public HudBarData GetAPData()
    {
        HudBarData apData;
        apData.CurrentValue = ActionPoints;
        apData.MaxValue = ActionPointsMax;

        return apData;
    }

    public HudBarData GetTPData()
    {
        HudBarData tpData;
        tpData.CurrentValue = TensionPoints;
        tpData.MaxValue = TensionPointsMax;

        return tpData;
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

    public bool GetFirstSpell(List<ERune> castedRunes, out Spell spellOut)
    {
        spellOut = new();
        foreach (Spell spell in SpellTome)
        {
            if(spell.IsSameRunes(castedRunes))
            {
                spellOut = spell;
                return true;
            }
        }

        return false;
    }
}
