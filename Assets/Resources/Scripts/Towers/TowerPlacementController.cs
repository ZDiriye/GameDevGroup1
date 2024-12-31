using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementController : MonoBehaviour
{
    private bool placed = false;
    public BaseTowerController tower;
    public GameObject hammer;
    private static TowerPlacementController activeHammer;
    private CursorManager cursorManager;

    private void OnMouseEnter()
    {
        CursorManager.Instance.SetPointingCursor();
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetDefaultCursor();
    }

    private void OnMouseDown()
    {
        if (!placed) // Only if the tower hasn't been placed yet
        {
            Debug.Log("Pressed");
            Vector3 hammerPosition = hammer.transform.position;
            GameplayUIController.instance.OpenTowerWheel(this, hammerPosition);
        }
    }

    public void TowerPlaced(BaseTowerController tower)
    {
        this.tower = tower;
        placed = true;

        if (hammer != null)
        {
            hammer.SetActive(false); // Disables the hammer GameObject
        }
    }

    public void TowerRemoved()
    {
        tower = null;
        placed = false;
    }
}