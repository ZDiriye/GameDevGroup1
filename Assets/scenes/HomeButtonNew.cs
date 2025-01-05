using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtonNew : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu"; // Set this to the name of your main menu scene in the Inspector

    public void GoToMainMenu()
    {
        Debug.Log("Button clicked! GoToMainMenu() triggered."); // Log to confirm method is called

        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.Log("Attempting to load scene: " + mainMenuSceneName); // Confirm scene name
            SceneManager.LoadScene(mainMenuSceneName); // Load the scene
        }
        else
        {
            Debug.LogError("Main menu scene name is not set or empty!"); // Error if no scene name
        }
    }
}
