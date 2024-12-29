using System.Collections;
using UnityEngine;

public class ArrowTowerController : BaseTowerController
{

    void OnDisable()
    {
        Debug.Log($"Tower {gameObject.name} was deactivated.");
    }

    void OnDestroy()
    {
        Debug.Log($"Tower {gameObject.name} was destroyed.");
    }

    // Shoots projectiles at the closest enemy with a cooldown.
    public override IEnumerator ShootTarget(Transform target)
    {
        shooting = true;

        while (shooting)
        {
            if (targetEnemy == null)
            {
                yield return null;
                continue;
            }

            // Instantiate and initialize the projectile
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = shootingPoint.position;
            projectile.transform.rotation = shootingPoint.rotation;
            projectile.GetComponent<Projectiles>().Initialise(targetEnemy.transform, 1.5f);

            // Wait for the cooldown duration
            yield return new WaitForSeconds(shootingCoolDown);
        }
    }
}
