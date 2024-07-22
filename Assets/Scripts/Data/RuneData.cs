using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ERune
{
    Right,
    Up,
    Down,
    Left,
    Air,
    Fire,
    Water
}

[Serializable]
public struct Rune
{
    public ERune Type;
    public Sprite Icon;
    public List<KeyCode> KCodes;
    public List<MouseButton> MButtons;
}

[CreateAssetMenu]
public class RuneData : ScriptableObject
{
    public Rune Value;
}
