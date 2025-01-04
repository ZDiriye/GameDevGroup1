using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public LevelController level;
    public static EnemyManager instance;
    public GameObject[] enemyPrefabs;
    public int maxEnemies;
    public float spawnIntervalMin; 
    public float spawnIntervalMax; 
    public Transform[] spawnpoint;
    private int activeEnemyCount = 0;
    [System.Serializable]
    public class NavigationPath
    {
        public List<Transform> waypoints = new List<Transform>();
    }

    public List<NavigationPath> navigationPaths = new List<NavigationPath>();

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
            Debug.LogError("More than one EnemyManager in the scene");
        }
    }

    public void SpawnEnemy(int enemyIndex, int pathIndex, bool strong, bool fast)
    {
        if (pathIndex >= navigationPaths.Count)
        {
            Debug.LogError("Path index out of range");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex]); 
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemy.transform.position = spawnpoint[pathIndex].position;
        
        // Convert the list of waypoints into an array for the Initialize method, if necessary
        enemyController.Initialise(navigationPaths[pathIndex].waypoints.ToArray(), strong, fast);
        
        activeEnemyCount++;
    }

    public void ReportEnemyDeath()
    {
        activeEnemyCount--;
        Debug.Log("Enemy died. Remaining: " + activeEnemyCount);
    }
}
