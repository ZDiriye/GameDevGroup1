using UnityEngine;
using UnityEngine.UI;

public class TowerWheelController : MonoBehaviour
{
    public Animator anim;
    public static int towerID; // Instance variable now
    public Camera cam;
    private bool isOpen = false;

    public void SetPositionAndOpen(Vector3 position)
    {
        if (!isOpen) // Open the menu only if it's not already open
        {
            Vector3 offsetPosition = position + new Vector3(0, 40.0f, 0f);
            transform.rotation = cam.transform.rotation;
            transform.position = offsetPosition;
            ToggleTowerWheel(true); // Open the menu
        }
    }

    public void ToggleTowerWheel(bool open)
    {
        anim.SetBool("OpenTowerWheel", open);
        isOpen = open;
    }

    public void Close()
    {
        if (isOpen)
        {
            // Pass the selected towerID to the GameplayUIController
            if (TowerWheelController.towerID > 0)
            {
                GameplayUIController.instance.PlaceSelectedTower(TowerWheelController.towerID);
            }
            ToggleTowerWheel(false);
        }
    }
}
