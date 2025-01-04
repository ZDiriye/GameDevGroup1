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
    public Material normalMaterial;
    public Material decalMaterial;
    public Renderer targetRenderer;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();
    }

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
            Vector3 hammerPosition = hammer.transform.position;
            GameplayUIController.instance.OpenTowerWheel(this, hammerPosition);
        }
    }

    public void TowerPlaced(BaseTowerController tower)
    {
        RemoveDecalMaterial();
        this.tower = tower;
        placed = true;

        TowerInteractionController interaction = tower.GetComponentInChildren<TowerInteractionController>();

        if (interaction != null)
        {
            interaction.placementController = this;
        }

        if (hammer != null)
        {
            hammer.SetActive(false);
        }
    }

    public void TowerRemoved()
    {
        ApplyDecalMaterial();
        tower = null;
        placed = false;

        if (hammer != null)
        {
            hammer.SetActive(true);
        }
    }

    public void ApplyDecalMaterial()
    {
       if (targetRenderer != null && decalMaterial != null)
        {
            targetRenderer.material = decalMaterial;
        }
    }

    public void RemoveDecalMaterial()
    {
        if (targetRenderer != null && normalMaterial != null)
        {
            targetRenderer.material = normalMaterial;
        }
    }
}