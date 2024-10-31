using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public GameObject ProjectilePrefab; // Projectile prefab
    private float FireRate = 1f; // Firing rate
    private float FireRange = 10f; // Shooting range
    private float fireCountdown = 0f; // Countdown for next shot
    private float TowerDamage = 100f; // Projectile damage
    private float TowerSpeed = 10f; // Projectile speed

    // Flag to check if tower is placed
    private bool isPlaced = false;

    void Update()
    {
        // Only shoot if the tower is placed
        if (!isPlaced)
        {
            return;
        }

        // Countdown to next shot
        fireCountdown -= Time.deltaTime;

        // Shoot if countdown reaches zero
        if (fireCountdown <= 0f)
        {
            Enemy target = GetNearestEnemy();
            if (target != null)
            {
                Shoot(target);
            }
            fireCountdown = 1f / FireRate;  // Reset countdown
        }
    }

    Enemy GetNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Enemy enemy in EntitySummoner.EnemiesInGame)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= FireRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    void Shoot(Enemy target)
    {
        // Create and set up projectile
        GameObject projectileGO = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetTarget(target);
            projectile.SetDamage(TowerDamage);
            projectile.SetSpeed(TowerSpeed);
        }
    }

    // Draw tower range in scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FireRange);
    }

    // Mark the tower as placed
    public void SetPlaced()
    {
        isPlaced = true;
    }
}
