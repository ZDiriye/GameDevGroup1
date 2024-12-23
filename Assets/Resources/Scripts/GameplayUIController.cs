using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    public HeadquarterController headquarters;
    public Image healthDisplay;


    // updates the user's health
    void Update()
    {
        float normalisedHealth = headquarters.health / 100.0f; 
        if (healthDisplay.fillAmount != normalisedHealth)
        {
            healthDisplay.fillAmount = normalisedHealth;
        }
        
    }
}
