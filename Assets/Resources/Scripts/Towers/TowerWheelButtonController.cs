using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class TowerWheelButtonController : MonoBehaviour
{
    public int id;
    public Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    private CursorManager cursorManager;

    private void OnMouseEnter()
    {
        CursorManager.Instance.SetPointingCursor();
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetDefaultCursor();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        itemText.text = "";

    }

    public void HoverEnter()
    {
        if (!anim.GetBool("Hover"))  // Only set hover if not already hovering
        {
            anim.SetBool("Hover", true);
            itemText.text = itemName;
        }
    }

    public void HoverExit()
    {
        if (anim.GetBool("Hover"))   // Only unset hover if currently hovering
        {
            anim.SetBool("Hover", false);
            itemText.text = "";
        }
    }
}
