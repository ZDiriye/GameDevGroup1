using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Singleton instance
    private AudioSource audioSource;     // Reference to the AudioSource
    private bool isMuted = false;        // Tracks mute state

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Load mute state from PlayerPrefs
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        if (audioSource != null)
        {
            audioSource.mute = isMuted;
        }
    }

    public static SoundManager GetInstance()
    {
        // If no instance exists, create one dynamically
        if (Instance == null)
        {
            GameObject soundManagerObject = new GameObject("SoundManager");
            Instance = soundManagerObject.AddComponent<SoundManager>();
            Instance.audioSource = soundManagerObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(soundManagerObject);
        }
        return Instance;
    }

    public void PlayMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        if (audioSource != null)
        {
            audioSource.mute = isMuted;
        }

        // Save the mute state
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool IsMuted()
    {
        return isMuted;
    }
}
