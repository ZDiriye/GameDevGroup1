using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image healthbar; // Reference to the UI Image representing the health bar
    public Camera cam; // Reference to the main camera for billboard effect
    public float target = 1; // Target fill amount for the health bar (normalized)
    public float reduceSpeed = 2; // Speed at which the health bar decreases visually

    public void Start()
    {
        cam = Camera.main; // Assign the main camera to ensure the health bar faces the camera
    }
    
    // Updates the target fill amount of the health bar based on current and max health
    public void UpdateHealthbar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth; // Calculate the normalized health value
    }

    void Update ()
    {
        // Rotate the health bar to always face the camera (billboarding effect)
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);

        // Smoothly update the health bar's fill amount towards the target value
        healthbar.fillAmount = Mathf.MoveTowards(healthbar.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
}
