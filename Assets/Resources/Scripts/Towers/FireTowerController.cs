using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTowerController : BaseTowerController
{
    public override IEnumerator ShootTarget(Transform target)
    {
        shooting = true;
        while (shooting)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.GetComponent<Projectiles>().Initialise(target, 1.5f);
            yield return new WaitForSeconds(shootingCoolDown);
        }

        StopShooting();
        yield return null;
    }
}
