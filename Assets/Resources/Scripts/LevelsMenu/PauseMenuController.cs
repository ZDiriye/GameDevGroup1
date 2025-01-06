using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PausePanel;

    public void Start()
    {
        PausePanel.SetActive(false);
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue() 
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("Continued");
    }
}
