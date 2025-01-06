using UnityEngine;
using UnityEngine.UI;

public class TutorialButtonController : MonoBehaviour
{
    // Reference to the tutorial panel
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Button toggleButton;

    void Start()
    {
        // Ensure the tutorial panel is initially hidden
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }

        // If using script to add listener
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleTutorialPanel);
        }
    }

    // Method to toggle the tutorial panel
    public void ToggleTutorialPanel()
    {
        if (tutorialPanel != null)
        {
            bool isActive = tutorialPanel.activeSelf;
            tutorialPanel.SetActive(!isActive);
        }
        else
        {
            Debug.LogWarning("Tutorial Panel is not assigned in the inspector.");
        }
    }
}
