using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Wave[] waves;  
    public int currentWaveIndex = 0;
    public Text waveCounterText;
    public Transform[] spawnPoints;
    public TimeSliderPositionController sliderController;
    private bool proceedToNextWave = false;
    private CursorManager cursorManager;
    public AudioSource countdownSound;

    private void OnMouseEnter()
    {
        CursorManager.Instance.SetPointingCursor();
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetDefaultCursor();
    }

    private void Start()
    {
        if (waves.Length == 0)
        {
            Debug.LogError("No waves defined in LevelManager.");
            return;
        }

        waveCounterText.text = $"{currentWaveIndex + 1}/{waves.Length}";
        sliderController.OnSliderClicked += HandleSliderClicked;
        ShowSliders();
        StartCoroutine(WaitForFirstWaveStart()); 
    }


    private IEnumerator RunWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            Debug.Log($"Starting Wave {currentWaveIndex + 1}");

            for (int enemyCount = 0; enemyCount < currentWave.numberOfEnemies; enemyCount++)
            {
                int enemyIndex = Random.Range(0, EnemyManager.instance.enemyPrefabs.Length);
                int pathIndex = Random.Range(0, EnemyManager.instance.spawnpoint.Length);
                bool isStrong = Random.value < currentWave.strongEnemyChance;
                bool isFast = Random.value < currentWave.fastEnemyChance;
                EnemyManager.instance.SpawnEnemy(enemyIndex, pathIndex, isStrong, isFast);
                
                yield return new WaitForSeconds(Random.Range(currentWave.spawnIntervalMin, currentWave.spawnIntervalMax));
            }

            while (EnemyManager.instance.ActiveEnemyCount > 0)
            {
                yield return null;
            }
            
            Debug.Log($"Wave {currentWaveIndex + 1} completed.");

            if (currentWaveIndex + 1 < waves.Length)  // Check if there's another wave before incrementing
            {
                currentWaveIndex++;
                waveCounterText.text = $"{currentWaveIndex + 1}/{waves.Length}";
                ShowSliders();

                proceedToNextWave = false;
                float elapsed = 0f;
                while (elapsed < sliderController.duration && !proceedToNextWave)
                {
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                sliderController.HideAllSliders();
            }
            else
            {
                currentWaveIndex++;  // To acknowledge the completion of the last wave
                break;  // Exit the loop as this is the last wave
            }
        }

        OnAllWavesCompleted();
    }


    private void OnAllWavesCompleted()
    {
        Debug.Log("All waves have been successfully completed!");
        // Implement any end-of-level logic here (e.g., show victory screen)
    }

    private void HandleSliderClicked()
    {
        proceedToNextWave = true;
    }

    private void ShowSliders()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            sliderController.MoveSliderTo(spawnPoint);
        }
        StartCoroutine(PlayCountdownSound());
    }

    private IEnumerator PlayCountdownSound()
    {
        countdownSound.Play();
        yield return new WaitForSeconds(20); 
        countdownSound.Stop(); 
    }

    private IEnumerator WaitForFirstWaveStart()
    {
        proceedToNextWave = false;
        float elapsed = 0f;
        while (!proceedToNextWave && elapsed < sliderController.duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        sliderController.HideAllSliders();
        StartCoroutine(RunWaves());
    }

    private void OnDestroy()
    {
        if (sliderController != null)
        {
            sliderController.OnSliderClicked -= HandleSliderClicked;
        }
    }
}
