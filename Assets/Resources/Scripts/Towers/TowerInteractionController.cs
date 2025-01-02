using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInteractionController : MonoBehaviour
{
    public BaseTowerController tower;
    private CursorManager cursorManager;
    public TowerPlacementController placementController;

    private void OnMouseEnter()
    {
        CursorManager.Instance.SetPointingCursor();
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetDefaultCursor();
    }


    protected virtual void OnMouseDown()
    {
        if (tower != null)
        {
            Vector3 towerPosition = tower.transform.position;
            GameplayUIController.instance.OpenUpgradeTowerWheel(this, towerPosition);
        }
    }
}
