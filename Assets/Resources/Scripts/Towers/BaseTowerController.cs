using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    protected float shootingCoolDown;
    protected Coroutine shoot;
    protected bool shooting;
    public Transform shootingPoint;
    protected List<Collider> enemiesInRange = new List<Collider>();
    protected Collider targetCollider;
    protected float cooldownTimer = 0f;

    // ShootTarget coroutine needs to be implemented by derived classes
    public virtual IEnumerator ShootTarget(Transform target)
    {
        throw new System.NotImplementedException();
    }

    protected virtual void Update()
    {
        // Decrement the cooldown timer
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        enemiesInRange.RemoveAll(enemy => enemy == null);
        Collider closestEnemy = GetClosestEnemy();

        if (closestEnemy != null && cooldownTimer <= 0)
        {
            if (targetCollider == null || closestEnemy != targetCollider)
            {
                // Switch to the new closest enemy and reset the cooldown
                SwitchTarget(closestEnemy);
            }
        }
        else if (shooting && closestEnemy == null)
        {
            // If no enemies are in range, stop shooting
            StopShooting();
        }
    }

    private void SwitchTarget(Collider newTarget)
    {
        targetCollider = newTarget;
        if (shooting)
        {
            StopCoroutine(shoot);
            shooting = false;
        }

        // Start shooting at the new target if the cooldown has elapsed
        if (cooldownTimer <= 0)
        {
            shoot = StartCoroutine(ShootTarget(newTarget.transform));
            cooldownTimer = shootingCoolDown; // Reset the cooldown timer
        }
    }


    // Finds the closest enemy from the enemiesInRange list
    protected Collider GetClosestEnemy()
    {
        Collider closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy == null) continue; // Skip if enemy has been destroyed

            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    // Detects enemies entering the tower's range
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!enemiesInRange.Contains(other))
            {
                enemiesInRange.Add(other);
                Debug.Log($"{gameObject.name}: Enemy entered range - {other.name}");
            }
        }
    }

    // Detects enemies exiting the tower's range
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInRange.Contains(other))
            {
                enemiesInRange.Remove(other);
                Debug.Log($"{gameObject.name}: Enemy exited range - {other.name}");
            }

            if (other == targetCollider)
            {
                StopShooting();
            }
        }
    }

    // Resets the shooting state and clears the current target
    protected virtual void StopShooting()
    {
        if (shoot != null)
        {
            StopCoroutine(shoot);
            shoot = null;
        }

        shooting = false;
        targetCollider = null;
        Debug.Log($"{gameObject.name}: Stopped shooting");
    }
}
