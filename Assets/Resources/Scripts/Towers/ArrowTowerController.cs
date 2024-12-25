using System.Collections;
using UnityEngine;

public class ArrowTowerController : BaseTowerController
{
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
