using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Function to load Level 1
    public void PlayGame()
    {
        Debug.Log("PlayGame() function is called by: " + gameObject.name);
        SceneManager.LoadScene("Level1");
    }

    // Function to quit the game
    public void ExitGame()
    {
        Debug.Log("ExitGame() function is called by: " + gameObject.name);
        Application.Quit();
    }
}
