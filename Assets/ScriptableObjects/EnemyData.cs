using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyType
{
    BAT
}

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [Serializable]
    public struct Enemy
    {
        public int id;
        public string Name;
        public int SpeedMovement;
        public int SpeedInitiative;
        public EEnemyType enemyTypeId; 
    }

    [SerializeField] public List<EnemyData.Enemy> EnemyList;
}
