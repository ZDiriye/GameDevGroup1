using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Singleton instance for global access to the SoundManager
    private AudioSource audioSource;     // Reference to the AudioSource component for managing sound playback
    private bool isMuted = false;        // Tracks whether the sound is muted

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the SoundManager persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate SoundManager instances
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the GameObject

        // Load the mute state from PlayerPrefs (default is unmuted if not set)
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        if (audioSource != null)
        {
            audioSource.mute = isMuted; // Apply the loaded mute state to the AudioSource
        }
    }

    public static SoundManager GetInstance()
    {
        // Ensure a single instance of SoundManager exists
        if (Instance == null)
        {
            GameObject soundManagerObject = new GameObject("SoundManager"); // Create a new GameObject
            Instance = soundManagerObject.AddComponent<SoundManager>();    // Add the SoundManager component
            Instance.audioSource = soundManagerObject.AddComponent<AudioSource>(); // Add an AudioSource component
            DontDestroyOnLoad(soundManagerObject); // Persist the new SoundManager across scenes
        }
        return Instance;
    }

    public void PlayMusic()
    {
        // Play the music if the AudioSource is valid and not already playing
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        // Stop the music if the AudioSource is valid and currently playing
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted; // Toggle the mute state

        if (audioSource != null)
        {
            audioSource.mute = isMuted; // Apply the mute state to the AudioSource
        }

        // Save the current mute state to PlayerPrefs for persistence
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool IsMuted()
    {
        return isMuted; // Return the current mute state
    }
}
