using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Wave[] waves;  // Array of waves, each containing enemy configurations
    public int currentWaveIndex = 0; // Tracks the current wave being processed
    public Text waveCounterText; // UI text to display the current wave
    public Transform[] spawnPoints; // Array of spawn points for enemies
    public TimeSliderPositionController sliderController; // Reference to slider controller for inter-wave transitions
    private bool proceedToNextWave = false; // Flag to determine if the next wave should start
    private CursorManager cursorManager; // Reference to the CursorManager for custom cursor states
    public AudioSource countdownSound; // Audio source for countdown sound effects

    public static string CurrentLevelName; // Stores the name of the current level

    private void OnMouseEnter()
    {
        // Change cursor to pointing state when hovering over certain objects
        CursorManager.Instance.SetPointingCursor();
    }

    private void OnMouseExit()
    {
        // Reset cursor to default state
        CursorManager.Instance.SetDefaultCursor();
    }

    private void Start()
    {
        // Ensure that waves are defined; otherwise, log an error and exit
        if (waves.Length == 0)
        {
            Debug.LogError("No waves defined in LevelManager.");
            return;
        }

        // Update the wave counter UI
        waveCounterText.text = $"{currentWaveIndex + 1}/{waves.Length}";
        sliderController.OnSliderClicked += HandleSliderClicked; // Subscribe to slider click events
        ShowSliders(); // Display sliders for the initial wave
        StartCoroutine(WaitForFirstWaveStart()); // Start the first wave after preparation

        // Record the current level name for tracking
        CurrentLevelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    private IEnumerator RunWaves()
    {
        // Iterate through all waves in the level
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            Debug.Log($"Starting Wave {currentWaveIndex + 1}");

            // Spawn all enemies for the current wave
            for (int enemyCount = 0; enemyCount < currentWave.numberOfEnemies; enemyCount++)
            {
                int enemyIndex = Random.Range(0, EnemyManager.instance.enemyPrefabs.Length); // Randomly select an enemy type
                int pathIndex = Random.Range(0, EnemyManager.instance.spawnpoint.Length); // Randomly select a spawn point
                bool isStrong = Random.value < currentWave.strongEnemyChance; // Determine if the enemy is strong
                bool isFast = Random.value < currentWave.fastEnemyChance; // Determine if the enemy is fast
                EnemyManager.instance.SpawnEnemy(enemyIndex, pathIndex, isStrong, isFast); // Spawn the enemy
                
                // Wait for a random interval before spawning the next enemy
                yield return new WaitForSeconds(Random.Range(currentWave.spawnIntervalMin, currentWave.spawnIntervalMax));
            }

            // Wait until all enemies in the wave are defeated
            while (EnemyManager.instance.ActiveEnemyCount > 0)
            {
                yield return null;
            }
            
            Debug.Log($"Wave {currentWaveIndex + 1} completed.");

            // Check if there are more waves to process
            if (currentWaveIndex + 1 < waves.Length)
            {
                currentWaveIndex++; // Move to the next wave
                waveCounterText.text = $"{currentWaveIndex + 1}/{waves.Length}"; // Update wave counter
                ShowSliders(); // Display sliders for the next wave

                proceedToNextWave = false;
                float elapsed = 0f;

                // Wait until the slider is clicked or the duration ends
                while (elapsed < sliderController.duration && !proceedToNextWave)
                {
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                sliderController.HideAllSliders(); // Hide sliders after preparation
            }
            else
            {
                currentWaveIndex++; // Acknowledge the last wave as completed
                break; // Exit the loop after the final wave
            }
        }

        OnAllWavesCompleted(); // Handle completion of all waves
    }

    private void OnAllWavesCompleted()
    {
        Debug.Log("All waves have been successfully completed!");
        SceneManager.LoadScene("LevelComplete"); // Load the "LevelComplete" scene after all waves
    }

    private void HandleSliderClicked()
    {
        proceedToNextWave = true; // Allow the next wave to start
    }

    private bool isCountdownPlaying = false; // Flag to track whether the countdown sound is playing
    private void ShowSliders()
    {
        // Move sliders to their respective spawn points
        foreach (Transform spawnPoint in spawnPoints)
        {
            sliderController.MoveSliderTo(spawnPoint);
        }

        // Play the countdown sound only if it is not already playing
        if (!isCountdownPlaying)
        {
            StartCoroutine(PlayCountdownSound());
        }
    }

    private IEnumerator PlayCountdownSound()
    {
        isCountdownPlaying = true; // Mark the countdown sound as active
        countdownSound.Play(); // Play the countdown sound
        yield return new WaitForSeconds(20); // Wait for 20 seconds
        countdownSound.Stop(); // Stop the countdown sound
        isCountdownPlaying = false; // Mark the countdown sound as inactive
    }

    private IEnumerator WaitForFirstWaveStart()
    {
        proceedToNextWave = false; // Reset the flag for wave progression
        float elapsed = 0f;

        // Wait until the slider is clicked or the duration ends
        while (!proceedToNextWave && elapsed < sliderController.duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        sliderController.HideAllSliders(); // Hide sliders before starting the waves
        StartCoroutine(RunWaves()); // Begin processing the waves
    }

    private void OnDestroy()
    {
        // Unsubscribe from slider click events to avoid potential errors
        if (sliderController != null)
        {
            sliderController.OnSliderClicked -= HandleSliderClicked;
        }
    }
}
