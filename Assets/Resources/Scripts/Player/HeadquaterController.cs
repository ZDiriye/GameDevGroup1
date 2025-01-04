using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HeadquarterController : MonoBehaviour
{
    public Renderer buildingRenderer;
    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;
    private Color originalColor;
    public float health;
    public Text UserHealthText;


    public void Start()
    {
        buildingRenderer = GetComponent<Renderer>();
        originalColor = buildingRenderer.material.GetColor("_Color");
        UserHealthText.text = health.ToString();
    }

    // for user's headquarters to take damage.
    public void Damage(float amount)
    {
        health -= amount;
        UserHealthText.text = health.ToString();

        StartCoroutine(FlashDamageEffect());

        if (health <= 0) 
        {
            Lose();
        }
    }

    // when the user looses.
    public void Lose()
    {

    }

    public void Heal(float amount)
    {
        health += amount;
        if (health >= 100)
        {
            health = 100;
        }
    }

    private IEnumerator FlashDamageEffect()
    {
        buildingRenderer.material.SetColor("_Color", hitColor);
        yield return new WaitForSeconds(flashDuration);
        buildingRenderer.material.SetColor("_Color", originalColor);
    }

}
