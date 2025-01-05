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
            Debug.LogWarning("Not enough currency!");
            ShowWarningMessage("Not enough currency!");
            return false;
        }
    }

    private void ShowWarningMessage(string message)
    {
        if (warningText != null)
        {
            warningText.text = message;
            warningText.gameObject.SetActive(true);
            backgroundImage.SetActive(true);
            Invoke("HideWarningMessage", 2.0f);  // Hide the message after 3 seconds
        }
    }

    private void HideWarningMessage()
    {
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
            backgroundImage.SetActive(false);
        }
    }
}
