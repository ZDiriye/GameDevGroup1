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
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = shootingPoint.position;
            projectile.transform.rotation = shootingPoint.rotation;
            // Log the time and the name of the projectile being shot
            Debug.Log($"[Time: {Time.time:F2}] Shooting {projectile.name} at {target.name} from {shootingPoint.position}");

            // Initialize the projectile to move towards the target
            projectile.GetComponent<Projectiles>().Initialise(target, 1.5f);

            // Wait for the cooldown period before shooting the next projectile
            yield return new WaitForSeconds(shootingCoolDown);

            // Log after waiting for the cooldown
            Debug.Log($"[Time: {Time.time:F2}] Next shot ready after cooldown of {shootingCoolDown} seconds");
        }
        yield return null;
    }
}