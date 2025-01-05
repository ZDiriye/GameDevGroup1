using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public RectTransform backgroundImage;
    public float parallaxIntensity = 10f;

    private Vector2 originalPosition;

    void Start()
    {
        originalPosition = backgroundImage.anchoredPosition;
    }

    void Update()
    {
        // get mouse pos in screen space
        Vector2 mousePosition = Input.mousePosition;

        // calc movement offset based on mouse pos
        float moveX = (mousePosition.x / Screen.width - 0.5f) * parallaxIntensity;
        float moveY = (mousePosition.y / Screen.height - 0.5f) * parallaxIntensity;

        // update background img pos
        backgroundImage.anchoredPosition = originalPosition + new Vector2(moveX, moveY);
    }
}
