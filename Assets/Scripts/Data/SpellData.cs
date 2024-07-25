using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spell
{
    public string Name;
    public string Description;
    public EElemental Type;
    public int BaseDamage;
    public int TensionCost;

    public EEffect EffectTrace;
    public EEffect EffectCollision;

    public bool IsJustTarget = false;

    public List<ERune> Runes;

    public bool IsSameRunes(List<ERune> otherRunes)
    {
        if (otherRunes.Count < Runes.Count ) return false;

        for (int i = 0; i < Runes.Count; i++)
        {
            if (otherRunes[i] != Runes[i]) return false;
        }

        return true;
    }

    public int GetDamage(EElemental elementalId)
    {
        int damage = BaseDamage;
        if ((elementalId == EElemental.Fire && Type == EElemental.Water)
            || (elementalId == EElemental.Water && Type == EElemental.Air)
            || (elementalId == EElemental.Air && Type == EElemental.Fire)
            )
            damage *= 2;

        if ((elementalId == EElemental.Fire && Type == EElemental.Air)
            || (elementalId == EElemental.Water && Type == EElemental.Fire)
            || (elementalId == EElemental.Air && Type == EElemental.Water)
            )
            damage = (int) (damage * 0.5f);

        if (damage <= 0) damage = 1;

        return damage;
    }
}

[CreateAssetMenu]
public class SpellData : ScriptableObject
{
    public Spell Value;

    
}
