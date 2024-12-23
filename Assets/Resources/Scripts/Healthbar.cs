using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image healthbar;
    public Camera cam;
    public float target = 1;
    public float reduceSpeed = 2;

    public void Start()
    {
        cam = Camera.main;
    }
    
    public void UpdateHealthbar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;
    }

    void Update ()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        healthbar.fillAmount = Mathf.MoveTowards(healthbar.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
}

