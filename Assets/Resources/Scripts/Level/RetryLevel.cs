using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RetryLevel : MonoBehaviour
{
    public void Retry()
    {   
        // Ensure the game is unfrozen before reloading the level
        Time.timeScale = 1;
        // Check if the current level name is set
        if (!string.IsNullOrEmpty(LevelManager.CurrentLevelName))
        {
            // Reload the saved level
            SceneManager.LoadScene(LevelManager.CurrentLevelName);
        }
        else
        {
            Debug.LogError("CurrentLevelName is not set. Cannot reload level.");
        }
    }
}
