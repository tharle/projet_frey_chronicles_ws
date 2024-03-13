using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    public string DisplayDamage();
    public string DisplayDescription();
    public bool IsAlive();

    public float GetRange();
}