using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void PlayAgain()
    {
        EntitySummoner.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        EntitySummoner.Reset();
        SceneManager.LoadScene("MainMenu");
    }
}