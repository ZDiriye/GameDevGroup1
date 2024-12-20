using UnityEngine;

public class HeadquarterController : MonoBehaviour
{
    public float health;

    // for user's headquarters to take damage
    public void Damage(float amount)
    {
        health -= amount;
        if (health <= 0) Lose();
    }

    // when the user looses
    public void Lose()
    {

    }

    public void Heal(float amount)
    {
        health += amount;
        if (health >= 100) health = 100;
    }

}
