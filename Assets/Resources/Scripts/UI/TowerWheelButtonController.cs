 using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class TowerWheelButtonController : MonoBehaviour
{
    public int id;
    public Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText, costText, descriptionText, damageText, cooldownText, upgradeCostText, sellPriceText;
    private CursorManager cursorManager;
    public GameObject towerPrefab, currentTowerPrefab;
    public bool showCost; 
    public bool useUpgradeUI; 
    public Image buttonImage,swordImage, timerImage, panel;

    private void OnMouseEnter()
    {
        CursorManager.Instance.SetPointingCursor();
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetDefaultCursor();
    }

    public void Start()
    {
        anim = GetComponent<Animator>();
        ClearUI(); 
    }

    public void SetTowerPrefab(GameObject newTowerPrefab, GameObject currentTowerPrefab)
    {
        towerPrefab = newTowerPrefab;
        this.currentTowerPrefab = currentTowerPrefab;
        ClearUI(); 
    }

    public void HoverEnter()
    {
        Debug.Log("HoverEnter called");
        if (!anim.GetBool("Hover"))
        {
            anim.SetBool("Hover", true);
            UpdateUI();
        }
    }

    public void HoverExit()
    {
        Debug.Log("HoverExit called");
        if (anim.GetBool("Hover"))   // Only unset hover if currently hovering
        {
            anim.SetBool("Hover", false);
            ClearUI();
        }
    }

    private void UpdateUI()
    {
        if (towerPrefab == null)
        {
            ClearUI();
            return;
        }

        var towerData = towerPrefab.GetComponent<BaseTowerController>();
        if (towerData == null)
        {
            ClearUI();
            return;
        }

        itemText.text = itemName;
        if (useUpgradeUI)
        {
            if (costText != null) costText.text = towerData.placementCost.ToString();
            if (sellPriceText != null) sellPriceText.text = "";
        }
        else if (showCost)
        {
            if (costText != null) costText.text = towerData.placementCost.ToString();
        }
        else
        {
            if (costText != null) costText.text = "";
            if (sellPriceText != null) sellPriceText.text = towerData.sellPrice.ToString();
        }

        descriptionText.text = towerData.description;
        damageText.text = towerData.damage.ToString();
        cooldownText.text = towerData.shootingCoolDown.ToString();

        if (currentTowerPrefab != null)
        {
            var currentTowerData = currentTowerPrefab.GetComponent<BaseTowerController>();
            if (currentTowerData != null && currentTowerData.nextTowerPrefab != null && upgradeCostText != null)
            {
                upgradeCostText.text = currentTowerData.nextTowerPrefab.upgradeCost.ToString();
            }
        }

        buttonImage.enabled = true;
        swordImage.enabled = true;
        timerImage.enabled = true;
        panel.enabled = true;
    }

    private void ClearUI()
    {
        itemText.text = "";
        if (costText != null) costText.text = "";
        if (descriptionText != null) descriptionText.text = "";
        if (damageText != null) damageText.text = "";
        if (cooldownText != null) cooldownText.text = "";
        if (upgradeCostText != null) upgradeCostText.text = "";
        if (sellPriceText != null) sellPriceText.text = "";
        buttonImage.enabled = false;
        swordImage.enabled = false;
        timerImage.enabled = false;
        panel.enabled = false;
    }
}