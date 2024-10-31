using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float Speed; 
    private Enemy target;
    private float Damage;

    // Target for the projectile
    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }

    // Damage for the projectile
    public void SetDamage(float Damage)
    {
        this.Damage = Damage;
    }

    // Speed for the projectile
    public void SetSpeed(float Speed)
    {
        this.Speed = Speed;
    }

    void Update()
    {
        // If there is no target, destroy the projectile
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate the direction to move the projectile towards the enemy
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float distanceThisFrame = Speed * Time.deltaTime;

        // Check if the projectile is close enough to hit the target
        float hitThreshold = 0.2f;
        if (Vector3.Distance(transform.position, target.transform.position) <= distanceThisFrame+hitThreshold)
        {
            HitTarget();  // Call the method to handle hitting the enemy
            return;
        }

        // Move the projectile towards the enemy
        transform.Translate(direction * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        // Remove the enemy when the projectile hits it
        if (target != null)
        {
            Debug.Log("Projectile hit target: " + target.ID);
            target.TakeDamage(Damage);
        }

        Destroy(gameObject);
    }
}