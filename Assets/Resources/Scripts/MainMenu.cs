using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Function to load Level 1
    public void PlayGame()
    {
        Debug.Log("PlayGame() function is called by: " + gameObject.name);
        SceneManager.LoadScene("LevelMenu");
    }
}
