using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EThingType
{
    DESTRUTIBLE,
    INDESTRUTIBLE
}
[Serializable]
public struct Thing : ITarget
{
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
public class ThingData : ScriptableObject
{
    public Thing Value;
}
