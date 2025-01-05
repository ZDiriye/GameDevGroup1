using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PausePanel;
    public AudioSource GameMusic;
    public AudioSource MenuMusic;
    private bool isPaused = false;

    void Start()
    {
        PausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Continue();
            else
                Pause();
        }
    }

    public void Pause()
    {
        Debug.Log("Game Paused");
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;

        if (GameMusic.isPlaying)
        {
            GameMusic.Pause();
            MenuMusic.Play(); 
        }
    }

    public void Continue()
    {
        Debug.Log("Game Continued");
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;

        if (!GameMusic.isPlaying) 
        {
            GameMusic.UnPause();
        }

        MenuMusic.Stop();
    }
}
