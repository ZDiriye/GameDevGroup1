using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Called when the Start Game button is pressed
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}