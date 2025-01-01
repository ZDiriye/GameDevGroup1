using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int numberOfEnemies;
    public float spawnIntervalMin;
    public float spawnIntervalMax;
    [Range(0, 1)]
    public float strongEnemyChance; // Probability an enemy is strong (e.g., 0.2 for 20%)
    [Range(0, 1)]
    public float fastEnemyChance;   // Probability an enemy is fast (e.g., 0.1 for 10%)
}


