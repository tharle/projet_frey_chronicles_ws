using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : TargetController
{
    private Enemy m_Enemy;
    public Enemy Enemy { set { m_Enemy = value; } }
}
