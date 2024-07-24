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
}

[CreateAssetMenu]
public class SpellData : ScriptableObject
{
    public Spell Value;

    
}
