using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public LevelController level; // Reference to the LevelController for accessing level-related data
    public static EnemyManager instance; // Singleton instance for global access to the EnemyManager
    public GameObject[] enemyPrefabs; // Array of enemy prefab references for spawning
    public int maxEnemies; // Maximum number of enemies allowed at a time
    public float spawnIntervalMin; // Minimum time interval between enemy spawns
    public float spawnIntervalMax; // Maximum time interval between enemy spawns
    public Transform[] spawnpoint; // Array of spawn points where enemies can spawn
    private int activeEnemyCount = 0; // Tracks the current number of active enemies

    // Represents a navigation path consisting of multiple waypoints
    [System.Serializable]
    public class NavigationPath
    {
        public List<Transform> waypoints = new List<Transform>(); // List of waypoints for the path
    }

    public List<NavigationPath> navigationPaths = new List<NavigationPath>(); // List of available navigation paths

    // Property to get the current number of active enemies
    public int ActiveEnemyCount 
    { 
        get { return activeEnemyCount; } 
    }

    public void Awake()
    {
        // Ensure only one EnemyManager exists in the scene
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one EnemyManager in the scene");
        }
    }

    // Spawns an enemy at a specified path and initializes it with properties
    public void SpawnEnemy(int enemyIndex, int pathIndex, bool strong, bool fast)
    {
        // Validate the path index to ensure it is within range
        if (pathIndex >= navigationPaths.Count)
        {
            Debug.LogError("Path index out of range");
            return;
        }

        // Instantiate the selected enemy prefab
        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex]); 
        EnemyController enemyController = enemy.GetComponent<EnemyController>(); // Get the EnemyController component
        enemy.transform.position = spawnpoint[pathIndex].position; // Set the spawn position

        // Initialize the enemy with its path and attributes (strong/fast)
        enemyController.Initialise(navigationPaths[pathIndex].waypoints.ToArray(), strong, fast);

        // Increment the count of active enemies
        activeEnemyCount++;
    }

    // Called when an enemy dies to decrement the active enemy count
    public void ReportEnemyDeath()
    {
        activeEnemyCount--; // Decrement the count of active enemies
        Debug.Log("Enemy died. Remaining: " + activeEnemyCount); // Log the current active enemy count
    }
}
