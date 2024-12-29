using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTowerController : BaseTowerController
{
    // Removed isReadyToShoot flag
    // private bool isReadyToShoot = true;

    public override IEnumerator ShootTarget(Transform target)
    {
        shooting = true;

        while (shooting)
        {
            // Clean up the enemies list
            enemiesInRange.RemoveAll(enemy => enemy == null || enemy.transform == null);

            if (enemiesInRange.Count > 0)
            {
                Vector3 bestTargetPosition = FindDensestClusterCenter(enemiesInRange);
                GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
                Projectiles p = projectile.GetComponent<Projectiles>();
                p.Initialise(bestTargetPosition, 2f);  // Targeting the predicted center of the densest cluster
            }
            else
            {
                // No enemies left, stop shooting
                shooting = false;
            }

            yield return new WaitForSeconds(shootingCoolDown);
        }

        // Coroutine ends, reset shoot reference
        shoot = null;
    }

    private Vector3 FindDensestClusterCenter(List<Collider> enemies)
    {
        Vector3 bestPosition = Vector3.zero;
        float bestDensity = float.MaxValue;
        float projectileSpeed = 10f; // Speed of your projectile

        foreach (var current in enemies)
        {
            Vector3 predictedPosition = PredictFuturePosition(current, projectileSpeed);

            float averageDistance = 0;
            foreach (var other in enemies)
            {
                Vector3 otherPredictedPosition = PredictFuturePosition(other, projectileSpeed);
                averageDistance += (predictedPosition - otherPredictedPosition).sqrMagnitude;
            }
            averageDistance /= enemies.Count;

            if (averageDistance < bestDensity)
            {
                bestDensity = averageDistance;
                bestPosition = predictedPosition;
            }
        }

        return bestPosition;
    }

    private Vector3 PredictFuturePosition(Collider enemy, float projectileSpeed)
    {
        Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
        if (enemyRigidbody == null)
        {
            // If no Rigidbody, assume the enemy is stationary
            return enemy.transform.position;
        }

        Vector3 enemyVelocity = enemyRigidbody.velocity;
        Vector3 enemyCurrentPosition = enemy.transform.position;

        // Calculate the time for the projectile to reach the enemy's current position
        float distanceToEnemy = Vector3.Distance(shootingPoint.position, enemyCurrentPosition);
        float timeToReach = distanceToEnemy / projectileSpeed;

        // Predict the enemy's position after that time
        return enemyCurrentPosition + (enemyVelocity * timeToReach);
    }
}
