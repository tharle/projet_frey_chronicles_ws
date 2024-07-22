using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ERune
{
    Right,
    Up,
    Down,
    Left
}

[Serializable]
public struct Rune
{
    public ERune Type;
    public Sprite Icon;
    public List<KeyCode> KCodes;
}

[CreateAssetMenu]
public class RuneData : ScriptableObject
{
    public Rune Value;
}
