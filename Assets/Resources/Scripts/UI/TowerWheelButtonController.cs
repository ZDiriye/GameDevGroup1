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
        if (towerPrefab != null)
        {
            var towerData = towerPrefab.GetComponent<BaseTowerController>();
            if (towerData != null)
            {
                itemText.text = itemName;
                costText.text = towerData.placementCost.ToString();
                descriptionText.text = towerData.description;
                damageText.text = towerData.damage.ToString();
                cooldownText.text = towerData.shootingCoolDown.ToString();
                if (currentTowerPrefab != null)
                {
                    var currentTowerData = currentTowerPrefab.GetComponent<BaseTowerController>();
                    if (upgradeCostText != null && currentTowerData != null && currentTowerData.nextTowerPrefab != null)
                    {
                        upgradeCostText.text = currentTowerData.nextTowerPrefab.upgradeCost.ToString();
                    }

                    sellPriceText.text = currentTowerData.sellPrice.ToString();
                    Debug.Log(currentTowerData.sellPrice.ToString());
                }
                buttonImage.enabled = true;
                swordImage.enabled = true;
                timerImage.enabled = true;
                panel.enabled = true;
            }
        }
        else
        {
            ClearUI();
        }
    }

    private void ClearUI()
    {
        itemText.text = "";
        if (costText != null)
        {
            costText.text = "";
        }
        descriptionText.text = "";
        if (damageText !=null)
        {
            damageText.text = "";
        }
        if (cooldownText != null)
        {
            cooldownText.text = "";
        }
        if (upgradeCostText != null)
        {
            upgradeCostText.text = ""; // Only clear this if it's not null
        }
        if (sellPriceText != null)
        {
            sellPriceText.text = ""; // Only clear this if it's not null
        }
        buttonImage.enabled = false;
        swordImage.enabled = false;
        timerImage.enabled = false;
        panel.enabled = false;
    }

}