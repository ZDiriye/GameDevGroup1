using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu"; // Set this to the name of your main menu scene in the Inspector

    public void GoToMainMenu()
    {
        // Debug to check if the method is called
        Debug.Log("GoToMainMenu() method called.");

        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            // Debug to check the scene name being attempted to load
            Debug.Log("Attempting to load scene: " + mainMenuSceneName);

            // Attempt to load the scene
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            // Debug error if the scene name is empty or null
            Debug.LogError("Main menu scene name is not set!");
        }
    }
}

