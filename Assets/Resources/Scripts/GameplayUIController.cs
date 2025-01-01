using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameplayUIController : MonoBehaviour
{
    public GameObject arrowTowerPrefab, iceTowerPrefab, bombTowerPrefab;
    public Button arrowTowerButton, iceTowerButton, bombTowerButton;
    public static GameplayUIController instance;
    public TowerPlacementController pointController;
    public TowerWheelController towerWheelController;
    private TowerPlacementController activeHammer;
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
        }
    }


    public void OpenTowerWheel(TowerPlacementController hammer, Vector3 position)
    {
        pointController = hammer;
        if (activeHammer == hammer)
        {
            CloseActiveMenu();
            return;
        }
        
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
    }


}