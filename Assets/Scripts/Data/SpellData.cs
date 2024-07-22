using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Spell
{
    public string Name;
    public string Description;
    public EElemental Type;
    public int BaseDamage;
    public int TensionCost;

    public EEffect EffectTrace;
    public EEffect EffectCollision;

    public List<ERune> Runes;
}

    [CreateAssetMenu]
public class SpellData : ScriptableObject
{
    public Spell Value;
}
