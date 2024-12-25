using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    public HeadquarterController headquarters;
    public Image healthDisplay;
    public GameObject arrowTowerPrefab, fireTowerPrefab, cannonTowerPrefab;
    public Button arrowTowerButton, fireTowerButton, cannonTowerButton;
    public static GameplayUIController instance;
    public TowerPlacementController pointController;
    public TowerWheelController towerWheelController;
    private TowerPlacementController activeHammer;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("More than one GameplayUIController in the scene");
        }
        SetUpButtons();
    }

    // Updates the user's health
    public void Update()
    {
        float normalisedHealth = headquarters.health / 100.0f; 
        if (healthDisplay.fillAmount != normalisedHealth)
        {
            healthDisplay.fillAmount = normalisedHealth;
        }

        if (Input.GetMouseButtonDown(0)) // Detect clicks outside
        {
            if (activeHammer != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Check if the click is outside the active hammer and menu
                if (Physics.Raycast(ray, out hit))
                {
                    if (!hit.transform.IsChildOf(activeHammer.transform) && 
                        !hit.transform.IsChildOf(towerWheelController.transform))
                    {
                        CloseActiveMenu();
                    }
                }
            }
        }
    }

    public void HandleHammerClick(TowerPlacementController hammer, Vector3 position)
    {
        pointController = hammer;
        if (activeHammer == hammer)
        {
            // If the same hammer is clicked, keep the menu open
            Debug.Log("Menu already open for this hammer.");
            return;
        }

        // Open menu for the clicked hammer
        OpenTowerWheel(hammer, position);
    }

    public void OpenTowerWheel(TowerPlacementController hammer, Vector3 position)
    {
        // Close the menu for the currently active hammer, if any
        if (activeHammer != null)
        {
            towerWheelController.Close();
            Debug.Log($"Menu closed for hammer: {activeHammer.gameObject.name}");
        }

        // Set the position and open the new menu
        towerWheelController.SetPositionAndOpen(position);
        activeHammer = hammer;
        Debug.Log($"Menu opened for hammer: {hammer.gameObject.name}");
    }

    public void CloseActiveMenu()
    {
        if (activeHammer != null)
        {
            towerWheelController.Close();
            Debug.Log($"Menu closed for hammer: {activeHammer.gameObject.name}");
            activeHammer = null;
        }
    }

    public void PlaceSelectedTower(int towerID)
    {
        GameObject towerPrefab = null;

        // Match the ID to the correct prefab
        switch (towerID)
        {
            case 1: // Example ID for Arrow Tower
                towerPrefab = fireTowerPrefab;
                break;
            case 2: // Example ID for Fire Tower
                towerPrefab = cannonTowerPrefab;
                break;
            case 3: // Example ID for Cannon Tower
                towerPrefab = arrowTowerPrefab;
                break;
            default:
                Debug.LogWarning("Invalid Tower ID selected.");
                return;
        }

        if (activeHammer != null && towerPrefab != null)
        {
            // Instantiate the selected tower at the hammer's position
            GameObject tower = Instantiate(towerPrefab);
            Vector3 adjustedPosition = pointController.transform.position;
            adjustedPosition.y -= 10.0f; // Move the tower down by 4.3 units
            tower.transform.position = adjustedPosition;
            pointController.TowerPlaced(tower.GetComponent<BaseTowerController>());

            Debug.Log($"Tower placed: {tower.name}");
        }
    }


    private void SetUpButtons()
    {
        fireTowerButton.onClick.AddListener(() =>
        {
            PlaceSelectedTower(1); // Fire Tower ID
            towerWheelController.Close();
        });

        cannonTowerButton.onClick.AddListener(() =>
        {
            PlaceSelectedTower(2); // Bomb Tower ID
            towerWheelController.Close();
        });

        arrowTowerButton.onClick.AddListener(() =>
        {
            PlaceSelectedTower(3); // Arrow Tower ID
            towerWheelController.Close();
        });
    }

}
 