using System.Collections;
using UnityEngine;

public class ArrowTowerController : BaseTowerController
{
    protected virtual void Awake()
    {
        shootingCoolDown = 2f;
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
            projectile.GetComponent<Projectiles>().Initialise(target, 1.5f);

            yield return new WaitForSeconds(shootingCoolDown);
            cooldownTimer = shootingCoolDown;
        }
    }
}
