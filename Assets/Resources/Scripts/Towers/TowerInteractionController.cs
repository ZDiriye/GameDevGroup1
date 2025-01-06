using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInteractionController : MonoBehaviour
{
    public BaseTowerController tower; // Reference to the tower this controller is interacting with
    private CursorManager cursorManager; // Reference to the CursorManager for changing cursor states
    public TowerPlacementController placementController; // Reference to the TowerPlacementController for managing placement

    private void OnMouseEnter()
    {
        // Change the cursor to a pointing cursor when the mouse hovers over the tower
        CursorManager.Instance.SetPointingCursor();
    }

    private void OnMouseExit()
    {
        // Reset the cursor to its default state when the mouse exits the tower
        CursorManager.Instance.SetDefaultCursor();
    }

    protected virtual void OnMouseDown()
    {
        if (tower != null)
        {
            // Get the position of the tower
            Vector3 towerPosition = tower.transform.position;

            // Open the UI for upgrading the tower at the clicked position
            GameplayUIController.instance.OpenUpgradeTowerWheel(this, towerPosition);
        }
    }
}
