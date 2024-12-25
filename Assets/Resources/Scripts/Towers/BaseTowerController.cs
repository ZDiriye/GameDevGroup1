using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootingCoolDown = 2f;
    protected bool shooting;
    protected Coroutine shoot;
    public Transform shootingPoint;

    private List<Collider> enemiesInRange = new List<Collider>(); // List of enemies in range
    protected Collider targetEnemy;

    // Initiates shooting at the target (to be implemented in derived classes).
    public virtual IEnumerator ShootTarget(Transform target)
    {
        throw new System.NotImplementedException();
    }

    // Checks for the closest enemy in range and updates the target.
    protected virtual void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            targetEnemy = GetClosestEnemy();
        }

        if (targetEnemy == null && shooting)
        {
            StopShooting();
        }
    }

    // Finds the closest enemy in range.
    protected Collider GetClosestEnemy()
    {
        float closestDistance = float.MaxValue;
        Collider closestEnemy = null;

        foreach (var enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    // Detects enemies entering the tower's range and adds them to the list.
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other);

            // Start shooting if no coroutine is running
            if (shoot == null)
            {
                shoot = StartCoroutine(ShootTarget(other.transform));
            }
        }
    }

    // Removes enemies leaving the tower's range from the list.
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other);

            // Stop shooting if the target leaves
            if (other == targetEnemy)
            {
                StopShooting();
            }
        }
    }

    // Resets the shooting state and clears the current target.
    protected virtual void StopShooting()
    {
        shooting = false;
        targetEnemy = null;

        // Stop the coroutine if it's running
        if (shoot != null)
        {
            StopCoroutine(shoot);
            shoot = null;
        }
    }
}
