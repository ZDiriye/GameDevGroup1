using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    public Texture2D defaultCursor; // Drag your default cursor texture here in the inspector
    public Texture2D pointingCursor; // Drag your pointing cursor texture here in the inspector

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Destroy the new instance if one already exists
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep the instance across scenes
        }
    }

    private void Start()
    {
        // Set the default cursor at the start
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetPointingCursor()
    {
        Cursor.SetCursor(pointingCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}
