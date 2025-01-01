using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Wave[] waves;  
    public int currentWaveIndex = 0;
    public Text waveCounterText;

    private void Start()
    {
        if (waves.Length == 0)
        {
            Debug.LogError("No waves defined in LevelManager.");
            return;
        }

        waveCounterText.text = $"{currentWaveIndex + 1}/{waves.Length}";
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            Debug.Log($"Starting Wave {currentWaveIndex + 1}");

            // if (currentWave.isBossWave && currentWave.bossPrefab != null)
            // {
            //     // Spawn the boss
            //     EnemyManager.instance.SpawnBoss(currentWave.bossPrefab);
            // }
            // else
            // {
            for (int enemyCount = 0; enemyCount < currentWave.numberOfEnemies; enemyCount++)
            {
                int enemyIndex = Random.Range(0, EnemyManager.instance.enemyPrefabs.Length);
                int pathIndex = Random.Range(0, EnemyManager.instance.spawnpoint.Length);

                bool isStrong = false;
                if (currentWave.strongEnemyChance > 0)
                {
                    isStrong = Random.value < currentWave.strongEnemyChance;
                }

                bool isFast = false;
                if (currentWave.fastEnemyChance > 0)
                {
                    isFast = Random.value < currentWave.fastEnemyChance;
                }

                EnemyManager.instance.SpawnEnemy(enemyIndex, pathIndex, isStrong, isFast);

                yield return new WaitForSeconds(Random.Range(currentWave.spawnIntervalMin, currentWave.spawnIntervalMax));
            }
            // }

            // Wait until all enemies in the wave are defeated
            while (EnemyManager.instance.ActiveEnemyCount > 0)
            {
                yield return null;
            }
            Debug.Log($"Wave {currentWaveIndex + 1} completed.");

            currentWaveIndex++;  // Move to next wave index here
            waveCounterText.text = $"{currentWaveIndex + 1}/{waves.Length}";
            // Optional: Wait before starting the next wave
            if (currentWaveIndex < waves.Length)  // Check if there's another wave
            {
                Debug.Log($"Waiting {20} seconds before next wave.");
                yield return new WaitForSeconds(20);
            }
        }
        // All waves completed
        OnAllWavesCompleted();
    }

    private void OnAllWavesCompleted()
    {
        Debug.Log("All waves have been successfully completed!");
        // Implement any end-of-level logic here (e.g., show victory screen)
    }
}
