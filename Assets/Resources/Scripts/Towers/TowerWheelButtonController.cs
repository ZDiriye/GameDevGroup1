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
    private bool selected = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        itemText.text = "";

    }

    void Update()
    {
        if (selected)
        {
            itemText.text = itemName;
        }
    }

    public void Selected()
    {
        selected = true;
        TowerWheelController.towerID = id;
        GameplayUIController.instance.towerWheelController.Close();
    }

    public void DeSelected()
    {
        selected = false;
        TowerWheelController.towerID = 0;
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        itemText.text = itemName;
    }


    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }

}
