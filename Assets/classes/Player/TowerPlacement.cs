using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    private GameObject CurrentPlacingTower;
    private PlayerMovement playerMovementScript;
    private int groundLayerMask;
    private int pathLayerMask;
    private float bufferDistance = 0.2f;
    public Text cannotPlaceText;

    void Start () 
    {
        playerMovementScript = GetComponent<PlayerMovement>();

        groundLayerMask = LayerMask.GetMask("Ground");
        pathLayerMask = LayerMask.GetMask("Path");

        if (cannotPlaceText != null)
        {
            cannotPlaceText.gameObject.SetActive(false); // Hide warning text initially
        }
    }

    void Update ()
    {
        if (CurrentPlacingTower != null)
        {
            Camera playerCamera = playerMovementScript.PlayerCamera.GetComponent<Camera>();
            Ray camray = playerCamera.ScreenPointToRay(Input.mousePosition);

            // Raycast to determine placement position, only on the "Ground" layer
            if (Physics.Raycast(camray, out RaycastHit hitInfo, 100f, groundLayerMask))
            {
                Bounds towerBounds = CurrentPlacingTower.GetComponent<Renderer>().bounds;

                // Adjust the Y position to place the tower on the ground
                Vector3 newPosition = hitInfo.point;
                newPosition.y += towerBounds.extents.y;

                CurrentPlacingTower.transform.position = newPosition;
            }

            // Place the tower if left mouse button is clicked
            if (Input.GetMouseButtonDown(0))
            {
                Bounds towerBounds = CurrentPlacingTower.GetComponent<Renderer>().bounds;
                Vector3 center = towerBounds.center;
                Vector3 expandedExtents = towerBounds.extents + new Vector3(bufferDistance, 0, bufferDistance);

                // Check for overlap with path before placing the tower
                Collider[] colliders = Physics.OverlapBox(center, expandedExtents, Quaternion.identity, pathLayerMask);

                if (colliders.Length > 0)
                {
                    ShowCannotPlaceWarning();
                }
                else
                {
                    // Finalise placement if it is a valid position
                    TowerBehaviour towerBehaviour = CurrentPlacingTower.GetComponent<TowerBehaviour>();
                    if (towerBehaviour != null)
                    {
                        towerBehaviour.SetPlaced(); 
                    }
                    CurrentPlacingTower = null;
                }
            }
        }
    }

    // Set a new tower to be placed by the player
    public void SetTowerToPlace(GameObject tower)
    {
        CurrentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }

    // Show the "Cannot Place Here" warning text on screen
    void ShowCannotPlaceWarning()
    {
        if (cannotPlaceText != null)
        {
            cannotPlaceText.gameObject.SetActive(true);
            StartCoroutine(HideCannotPlaceWarning()); // Hide warning after delay
        }
    }

    // Hide the "Cannot Place Here" warning text after a short delay
    IEnumerator HideCannotPlaceWarning()
    {
        yield return new WaitForSeconds(1f);
        if (cannotPlaceText != null)
        {
            cannotPlaceText.gameObject.SetActive(false);
        }
    }
}