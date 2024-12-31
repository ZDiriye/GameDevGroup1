using System.Collections;
using UnityEngine;

public class BombTowerController : BaseTowerController
{
    [SerializeField] private GameObject PoofEffect; 
    protected virtual void Awake()
    {
        shootingCoolDown = 3.5f;
        Debug.Log($"{gameObject.name}: Awake - shootingCoolDown set to {shootingCoolDown}");
    }

     /// Selects the nearest enemy as the target.
    protected override Collider SelectTarget()
    {
        return CalculateHighestDensityCluster();
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
            projectile.GetComponent<Projectiles>().Initialise(target.position, 1.5f);

            Instantiate(PoofEffect, shootingPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(shootingCoolDown);
            cooldownTimer = shootingCoolDown;
        }
    }
}
