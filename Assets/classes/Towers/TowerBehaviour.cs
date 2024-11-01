using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public GameObject ProjectilePrefab; 
    private float FireRate = 0.5f; 
    private float FireRange = 10f; 
    private float fireCountdown = 0f; 
    private float TowerDamage = 50f; 
    private float TowerSpeed = 10f; 
    private bool isPlaced = false; 

    void Update()
    {
        // Only shoot if the tower is placed and the game has not stopped
        if (!isPlaced || !GameLoopManager.GameIsActive)
        {
            return;
        }

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
            // Skip enemies that are already being targeted
            if (enemy.IsTargeted)
            {
                continue;
            }

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= FireRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // Mark the enemy as targeted if found
        if (nearestEnemy != null)
        {
            nearestEnemy.IsTargeted = true;
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

    // Mark the tower as placed
    public void SetPlaced()
    {
        isPlaced = true;
    }
}
