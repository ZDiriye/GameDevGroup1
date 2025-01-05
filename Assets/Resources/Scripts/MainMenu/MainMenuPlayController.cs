using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPlayController : MonoBehaviour
{
    public string sceneToLoad = "NameOfYourScene";
    public AudioSource buttonClickSound;

    // This method is called when the button is clicked
    public void OnPlayButtonPressed()
    {
        Debug.Log("Play button clicked");
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }

        // // Delay scene loading slightly to let the sound play
        // Invoke(nameof(LoadScene), buttonClickSound.clip.length);
    }

    // // Separate method for loading the scene
    // private void LoadScene()
    // {
    //     SceneManager.LoadScene(sceneToLoad);
    // }
}
