using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTowerController : BaseTowerController
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
            projectile.GetComponent<Projectiles>().Initialise(target.position, 1.5f);
            yield return new WaitForSeconds(shootingCoolDown);
        }
        yield return null;
    }
}