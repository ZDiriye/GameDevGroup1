using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance for global access to the AudioManager

    public AudioSource musicSource; // Reference to the AudioSource component for playing music

    private bool isMuted = false; // Tracks whether the audio is muted

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the AudioManager persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate AudioManager instances
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted; // Toggle the mute state
        musicSource.mute = isMuted; // Apply the mute state to the music source
    }

    public bool IsMuted()
    {
        return isMuted; // Return the current mute state
    }

    private void Start()
    {
        // Load the saved mute state from PlayerPrefs when the application starts
        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
        musicSource.mute = isMuted; // Apply the loaded mute state to the music source
    }

    private void OnApplicationQuit()
    {
        // Save the current mute state to PlayerPrefs when the application quits
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
    }
}
