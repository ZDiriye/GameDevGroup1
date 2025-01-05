using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerWheelController : MonoBehaviour
{
    public Animator anim;
    public Camera cam;
    private bool isOpen = false;
    private CursorManager cursorManager;

    private void OnMouseEnter()
    {
        CursorManager.Instance.SetPointingCursor();
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetDefaultCursor();
    }


    public void SetPositionAndOpen(Vector3 position)
    {
        if (!isOpen) // Open the menu only if it's not already open
        {
            // Convert screen center to world position
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane);
            Vector3 worldCenter = cam.ScreenToWorldPoint(screenCenter);

            // Offset the wheel slightly toward the camera's center
            transform.position = position + (worldCenter - position) * 0.1f;
            transform.position += cam.transform.up * 10f;
            transform.position += cam.transform.right * -2f;

            // Ensure the wheel faces the camera
            transform.LookAt(transform.position + cam.transform.forward);

            ToggleTowerWheel(true); // Open the menu
        }
    }
    public void SetPositionAndOpenUpgrade(Vector3 position)
    {
        if (!isOpen) // Open the menu only if it's not already open
        {
            // Convert screen center to world position
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane);
            Vector3 worldCenter = cam.ScreenToWorldPoint(screenCenter);

            transform.position = position;
            transform.position += cam.transform.up * 20f;       
            transform.position += cam.transform.forward * -10f;  
            transform.LookAt(transform.position + cam.transform.forward);
            ToggleUpgradeTowerWheel(true);
        }
    }

    public void ToggleTowerWheel(bool open)
    {
        anim.SetBool("OpenTowerWheel", open);
        isOpen = open;
        RefreshEventSystem();
    }

    public void ToggleUpgradeTowerWheel(bool open)
    {
        anim.SetBool("OpenUpgradeTowerWheel", open);
        isOpen = open;
        RefreshEventSystem();
    }

    public void Close()
    {
        if (isOpen)
        {
            ToggleTowerWheel(false);
        }
    }

    public void CloseUpgrade()
    {
        if (isOpen)
        {
            ToggleUpgradeTowerWheel(false);
        }
    }

    public void RefreshEventSystem()
    {
        EventSystem.current.SetSelectedGameObject(null);

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        EventSystem.current.SetSelectedGameObject(pointerData.pointerCurrentRaycast.gameObject);
    }
}