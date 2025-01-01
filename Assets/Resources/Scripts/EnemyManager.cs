using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public LevelController level;
    public Transform[] navigationPath0, navigationPath1;
    public static EnemyManager instance;
    public GameObject[] enemyPrefabs;
    public int maxEnemies;
    public float spawnIntervalMin; 
    public float spawnIntervalMax; 
    public Transform [] spawnpoint;
    private int activeEnemyCount = 0;

    public int ActiveEnemyCount 
    { 
        get { return activeEnemyCount; } 
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("More than one EnemyManager in the scene");
        }
    }

    public void SpawnEnemy(int enemyIndex, int pathIndex, bool strong, bool fast)
    {
        
        GameObject enemy =  Instantiate(enemyPrefabs[enemyIndex]); // creates the object for that enemy
        EnemyController enemyController =  enemy.GetComponent<EnemyController>();

        if (pathIndex == 0)
        {
            enemy.transform.position = spawnpoint[0].position;
            enemyController.Initialise(navigationPath0, strong, fast);
        }
        else
        {
            enemy.transform.position = spawnpoint[1].position;
            enemyController.Initialise(navigationPath1, strong, fast);
        }
        activeEnemyCount++;
    }

    public void ReportEnemyDeath()
    {
        activeEnemyCount--;
        Debug.Log("Enemy died. Remaining: " + activeEnemyCount);
    }
}
