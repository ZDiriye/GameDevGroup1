using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public float shootingCoolDown;
    protected Coroutine shoot;
    protected bool shooting;
    protected List<Collider> enemiesInRange = new List<Collider>();
    protected Collider targetCollider;
    protected float cooldownTimer;
    protected float densityRadius = 15f;
    public int damage;
    public int aoe;
    public float speed;
    public BaseTowerController nextTowerPrefab;
    public int placementCost;
    public string towerName;
    public int upgradeCost; 
    public int sellPrice;
    public string description;
    public float percentage;
    

    /// Coroutine for shooting at the target. To be implemented by derived classes.
    public virtual IEnumerator ShootTarget(Transform target)
    {
        throw new System.NotImplementedException();
    }

    protected virtual void Update()
    {
        // Update cooldown timer
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        // Remove null references
        enemiesInRange.RemoveAll(enemy => enemy == null);

        // Select target based on derived class logic
        Collider selectedTarget = SelectTarget();

        if (selectedTarget != null && cooldownTimer <= 0)
        {
            if (targetCollider == null || selectedTarget != targetCollider)
            {
                SwitchTarget(selectedTarget);
            }
        }
        else if (shooting && selectedTarget == null)
        {
            StopShooting();
        }
        Debug.Log($"Currently tracking {enemiesInRange.Count} enemies.");
    }

    /// Selects the target based on specific tower logic. Override in derived classes.
    protected virtual Collider SelectTarget()
    {
        throw new System.NotImplementedException();
    }

    /// Switches the current target and starts shooting.
    private void SwitchTarget(Collider newTarget)
    {
        targetCollider = newTarget;

        if (shooting)
        {
            StopCoroutine(shoot);
            shooting = false;
        }

        if (cooldownTimer <= 0)
        {
            shoot = StartCoroutine(ShootTarget(newTarget.transform));
            cooldownTimer = shootingCoolDown;
        }
    }

    protected Collider CalculateHighestDensityCluster()
    {
        if (enemiesInRange.Count == 0) return null;

        Collider bestTarget = null;
        int maxDensity = 0;

        // Iterate over each enemy to find where the most enemies are clustered around it
        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy == null) continue;
            int densityCount = 0;

            foreach (Collider other in enemiesInRange)
            {
                if (other == null) continue;
                if (Vector3.Distance(enemy.transform.position, other.transform.position) <= densityRadius)
                    densityCount++;
            }

            // Select the enemy that has the highest number of neighbors within the density radius
            if (densityCount > maxDensity)
            {
                maxDensity = densityCount;
                bestTarget = enemy;
            }
        }

        return bestTarget;
    }

    /// Finds the closest enemy from the enemiesInRange list.
    protected Collider GetClosestEnemy()
    {
        Collider closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    /// Handles enemies entering the tower's range.
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !enemiesInRange.Contains(other))
        {
            enemiesInRange.Add(other);
            Debug.Log($"{gameObject.name}: Enemy entered range - {other.name}");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Enemy already in range - {other.name}");
        }
    }

    /// Handles enemies exiting the tower's range.
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && enemiesInRange.Contains(other))
        {
            enemiesInRange.Remove(other);
            Debug.Log($"{gameObject.name}: Enemy exited range - {other.name}");
        }

        if (other == targetCollider)
        {
            StopShooting();
        }
    }

    /// Stops shooting and resets the current target.
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
