using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Spell
{
    public string Name;
    public string Description;
    // TODO add comands
    public EElemental Type;
    public int BaseDamage;
    public int TensionCost;
}

    [CreateAssetMenu]
public class SpellData : ScriptableObject
{
    public Spell Value;
}
