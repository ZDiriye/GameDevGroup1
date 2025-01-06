using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    public int CurrentCurrency { get; private set; } = 300; // Starting currency
    public Text currencyText;
    public TextMeshProUGUI warningText; 
    public GameObject backgroundImage;

    private bool isWarningActive = false; // Tracks if a warning is currently active

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        HideWarningMessage();
    }

    private void Start()
    {
        UpdateCurrencyUI();
    }

    public bool SpendCurrency(int amount)
    {
        if (CurrentCurrency >= amount)
        {
            CurrentCurrency -= amount;
            UpdateCurrencyUI();
            return true;
        }
        else
        {
            if (!isWarningActive) // Prevent overlapping warnings
            {
                Debug.LogWarning("Not enough currency!");
                ShowWarningMessage("Not enough currency!");
            }
            return false;
        }
    }

    private void ShowWarningMessage(string message)
    {
        isWarningActive = true; // Indicate that a warning is active

        if (warningText != null)
        {
            warningText.text = message;
            warningText.gameObject.SetActive(true);
            backgroundImage.SetActive(true);

            // Hide the message after 2 seconds
            Invoke("HideWarningMessage", 2.0f);
        }
    }

    private void HideWarningMessage()
    {
        isWarningActive = false; // Reset warning state

        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
            backgroundImage.SetActive(false);
        }
    }

    public void AddCurrency(int amount)
    {
        CurrentCurrency += amount;
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
        {
            currencyText.text = CurrentCurrency.ToString();
        }
    }
}
