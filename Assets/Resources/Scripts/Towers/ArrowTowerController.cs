using System.Collections;
using UnityEngine;

public class ArrowTowerController : BaseTowerController
{

    /// Selects the nearest enemy as the target.
    protected override Collider SelectTarget()
    {
        return GetClosestEnemy();
    }

    /// Coroutine to shoot arrows at the targeted enemy.
    public override IEnumerator ShootTarget(Transform target)
    {
        shooting = true;
        while (shooting)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = shootingPoint.position;
            projectile.transform.rotation = shootingPoint.rotation;
            projectile.GetComponent<Projectiles>().Initialise(target, speed, damage);

            yield return new WaitForSeconds(shootingCoolDown);
            cooldownTimer = shootingCoolDown;
        }
    }
}
