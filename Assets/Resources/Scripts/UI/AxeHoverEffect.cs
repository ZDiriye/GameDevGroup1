using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeHoverEffect : MonoBehaviour
{
    public float hoverHeight; // How high it moves up and down
    public float hoverSpeed;   // How fast it moves up and down

    private Vector3 startPosition;

    void Start()
    {
        // Save the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}


