using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class TowerWheelButtonController : MonoBehaviour
{
    public int id;
    public Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI cooldownText;
    private CursorManager cursorManager;
    public GameObject towerPrefab;
    public bool showCost; 
    public Image buttonImage;
    public Image swordImage;
    public Image timerImage;
    public Image panel;

    private void OnMouseEnter()
    {
        CursorManager.Instance.SetPointingCursor();
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetDefaultCursor();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        itemText.text = "";
        if (costText != null && showCost && towerPrefab != null) 
        {
            var towerData = towerPrefab.GetComponent<BaseTowerController>();
            if (towerData != null)
            {
                costText.text = "";
                descriptionText.text = "";
                damageText.text = "";
                cooldownText.text = "";
            }
            buttonImage.enabled = false;
            panel.enabled = false;
            swordImage.enabled = false;
            timerImage.enabled = false;
        }

    }

    public void HoverEnter()
    {
        if (!anim.GetBool("Hover"))
        {
            anim.SetBool("Hover", true);
            itemText.text = itemName;

            if (showCost && towerPrefab != null)
            {
                var towerData = towerPrefab.GetComponent<BaseTowerController>();
                if (towerData != null)
                {
                    costText.text = towerData.placementCost.ToString();
                    descriptionText.text = towerData.description;
                    damageText.text = towerData.damage.ToString();
                    cooldownText.text = towerData.shootingCoolDown.ToString();
                }
                buttonImage.enabled = true;
                swordImage.enabled = true;
                timerImage.enabled = true;
                panel.enabled = true;
            }
        }
        
    }

    public void HoverExit()
    {
        if (anim.GetBool("Hover"))   // Only unset hover if currently hovering
        {
            anim.SetBool("Hover", false);
            itemText.text = "";
            costText.text = "";
            buttonImage.enabled = false;
            descriptionText.text = "";
            damageText.text = "";
            cooldownText.text = "";
            panel.enabled = false;
            swordImage.enabled = false;
            timerImage.enabled = false;
        }
    }
}
