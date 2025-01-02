using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameplayUIController : MonoBehaviour
{
    public GameObject arrowTowerPrefab, iceTowerPrefab, bombTowerPrefab;
    public Button arrowTowerButton, iceTowerButton, bombTowerButton, upgradeButton;
    public static GameplayUIController instance;
    public TowerPlacementController pointController, activeHammer;
    public TowerInteractionController towerController, activeTower;
    public TowerWheelController towerWheelController;
    public TowerWheelController UpgradetowerWheelController;

    private bool isPlacingTower = false;

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
        // Detect clicks outside UI elements
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // Check if the click is not on a UI element
        {
            if (activeHammer != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Check if the click is outside the active hammer and menu
                if (Physics.Raycast(ray, out hit) &&
                    (!hit.transform.IsChildOf(activeHammer.transform) &&
                    !hit.transform.IsChildOf(towerWheelController.transform)))
                {
                    CloseActiveMenu();
                }
            }

            if (activeTower != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Check if the click is outside the active hammer and menu
                if (Physics.Raycast(ray, out hit) &&
                    (!hit.transform.IsChildOf(activeTower.transform) &&
                    !hit.transform.IsChildOf(UpgradetowerWheelController.transform)))
                {
                    CloseUpgradeActiveMenu();
                }
            }
        }
    }


    public void OpenTowerWheel(TowerPlacementController hammer, Vector3 position)
    {
        if (activeTower != null)
        {
            UpgradetowerWheelController.CloseUpgrade();
            activeTower = null;
        }

        pointController = hammer;
        if (activeHammer == hammer)
        {
            CloseActiveMenu();
            return;
        }
        
        if (activeHammer != null)
        {
            towerWheelController.Close();
            Debug.Log($"Menu closed for hammer: {activeHammer.gameObject.name}");
        }

        towerWheelController.SetPositionAndOpen(position);
        activeHammer = hammer;
        Debug.Log($"Menu opened for hammer: {hammer.gameObject.name}");
    }


    public void OpenUpgradeTowerWheel(TowerInteractionController tower, Vector3 position)
    {
        if (activeHammer != null)
        {
            towerWheelController.Close();
            activeHammer = null;
        }

        towerController = tower;
        if (activeTower == tower)
        {
            CloseUpgradeActiveMenu();
            return;
        }
        
        if (activeTower != null)
        {
            UpgradetowerWheelController.CloseUpgrade();
        }

        UpgradetowerWheelController.SetPositionAndOpenUpgrade(position);
        activeTower = tower;
    }


    public void CloseUpgradeActiveMenu()
    {
        UpgradetowerWheelController.CloseUpgrade();
        activeTower = null;
    }

    public void CloseActiveMenu()
    {
        towerWheelController.Close();
        activeHammer = null;
    }

    public void PlaceSelectedTower(int towerID)
    {
        if (isPlacingTower)
        {
            Debug.Log("Attempt to place a tower while another placement is in progress.");
            return;
        }

        isPlacingTower = true;
        GameObject towerPrefab = null;

        switch (towerID)
        {
            case 1:
                towerPrefab = iceTowerPrefab;
                break;
            case 2:
                towerPrefab = bombTowerPrefab;
                break;
            case 3:
                towerPrefab = arrowTowerPrefab;
                break;
            default:
                Debug.LogWarning("Invalid Tower ID selected.");
                return;
        }

        if (activeHammer != null && towerPrefab != null)
        {
            GameObject tower = Instantiate(towerPrefab);
            Vector3 adjustedPosition = pointController.transform.position;
            adjustedPosition.y -= 5.0f;  // Ensure this value is correct
            tower.transform.position = adjustedPosition;

            Debug.Log($"Tower placed: {tower.name} at {tower.transform.position}");

            pointController.TowerPlaced(tower.GetComponent<BaseTowerController>());
        }
        else
        {
            Debug.Log("Active hammer not set or prefab not found.");
        }

        Debug.Log("Tower ID: " + towerID);
        Debug.Log("Active Hammer: " + (activeHammer != null).ToString());
        Debug.Log("Tower Prefab: " + (towerPrefab != null).ToString());
        Debug.Log("Point Controller Position: " + pointController.transform.position);


        isPlacingTower = false;
    }

    public void UpgradeTower(BaseTowerController currentTower, TowerPlacementController placementController)
    {
        // Check if there is a next tower
        if (currentTower.nextTowerPrefab == null)
        {
            Debug.Log("No more upgrades available for this tower.");
            return;
        }

        Debug.Log("Upgraded Tower");
        Vector3 oldPosition = currentTower.transform.position;

        Destroy(currentTower.gameObject);
        BaseTowerController newTower = Instantiate(currentTower.nextTowerPrefab, oldPosition, Quaternion.identity);
        placementController.TowerPlaced(newTower);

    }



    private void SetUpButtons()
    {
        Debug.Log("Setting up buttons...");

        iceTowerButton.onClick.AddListener(() =>
        {
            Debug.Log("Fire Tower Button Clicked");
            PlaceSelectedTower(1);
            CloseActiveMenu();
        });

        bombTowerButton.onClick.AddListener(() =>
        {
            Debug.Log("Cannon Tower Button Clicked");
            PlaceSelectedTower(2);
            CloseActiveMenu();
        });

        arrowTowerButton.onClick.AddListener(() =>
        {
            Debug.Log("Arrow Tower Button Clicked");
            PlaceSelectedTower(3);
            CloseActiveMenu();
        });

        upgradeButton.onClick.AddListener(() =>
        {
            Debug.Log("Upgrade Button Clicked");
            if (activeTower != null && activeTower.tower != null && activeTower.placementController != null)
            {
                UpgradeTower(activeTower.tower, activeTower.placementController);
            }
            CloseUpgradeActiveMenu();
        });
    }
}