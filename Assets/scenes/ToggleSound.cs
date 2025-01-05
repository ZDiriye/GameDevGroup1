using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for the Image type

public class ToggleSound : MonoBehaviour
{
    public Sprite soundOnIcon;  // Drag your "Sound On" sprite here in the Inspector
    public Sprite soundOffIcon; // Drag your "Sound Off" sprite here in the Inspector
    public Vector2 soundOnSize = new Vector2(50, 50); // Default size for "Sound On"
    public Vector2 soundOffSize = new Vector2(50, 50); // Default size for "Sound Off"

    private bool isSoundOn = true; // Default state
    private Image buttonImage;
    private RectTransform buttonRect;

    void Start()
    {
        // Get the Image and RectTransform components of the button
        buttonImage = GetComponent<Image>();
        buttonRect = GetComponent<RectTransform>();

        // Set default sprite and size
        buttonImage.sprite = soundOnIcon;
        buttonRect.sizeDelta = soundOnSize;
    }

    public void OnToggleSound() // Renamed to avoid conflict with class name
    {
        isSoundOn = !isSoundOn; // Toggle state

        if (isSoundOn)
        {
            buttonImage.sprite = soundOnIcon; // Set "Sound On" sprite
            buttonRect.sizeDelta = soundOnSize; // Adjust size for "Sound On"
            // Enable sound functionality here, e.g., AudioListener.volume = 1;
        }
        else
        {
            buttonImage.sprite = soundOffIcon; // Set "Sound Off" sprite
            buttonRect.sizeDelta = soundOffSize; // Adjust size for "Sound Off"
            // Disable sound functionality here, e.g., AudioListener.volume = 0;
        }
    }
}
