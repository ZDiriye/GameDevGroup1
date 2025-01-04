using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;

    private bool isMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        musicSource.mute = isMuted;
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    private void Start()
    {
        // Load saved mute state
        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
        musicSource.mute = isMuted;
    }

    private void OnApplicationQuit()
    {
        // Save mute state
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
    }
}
