using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTowerController : BaseTowerController
{
    protected virtual void Awake()
    {
        shootingCoolDown = 3.5f;
        Debug.Log($"{gameObject.name}: Awake - shootingCoolDown set to {shootingCoolDown}");
    }

    // Shoots projectiles at the current target with a cooldown.
    public override IEnumerator ShootTarget(Transform target)
    {
        shooting = true;
        while (shooting)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = shootingPoint.position;
            projectile.transform.rotation = shootingPoint.rotation;
            projectile.GetComponent<Projectiles>().Initialise(target.position, 1.5f);
            
            cooldownTimer = shootingCoolDown;
            yield return new WaitForSeconds(shootingCoolDown);
        }
    }
}