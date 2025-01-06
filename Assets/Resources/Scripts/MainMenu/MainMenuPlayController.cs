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
    }
}
