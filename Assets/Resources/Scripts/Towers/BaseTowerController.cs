using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    protected float shootingCoolDown;
    protected Coroutine shoot;
    protected bool shooting;
    public Transform shootingPoint;
    protected Collider targetCollider;

    protected virtual void Awake()
    {
        shootingCoolDown = 2;
        Debug.Log($"{gameObject.name}: Awake - shootingCoolDown set to {shootingCoolDown}");
    }
    public virtual IEnumerator ShootTarget(Transform target)
    {
        throw new System.NotImplementedException();
    }

    // Checks for the closest enemy in range and updates the target.
    protected virtual void Update()
    {
        if (targetCollider == null && shooting)
        {
            StopShooting();
        }
    }

    // Detects enemies entering the tower's range and adds them to the list.
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && shoot == null)
        {
            targetCollider = other;
            shoot = StartCoroutine(ShootTarget(other.transform));
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && shoot == null)
        {
            targetCollider = other;
            shoot = StartCoroutine(ShootTarget(other.transform));
        }
    }

    // Removes enemies leaving the tower's range from the list.
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other == targetCollider)
            {
                StopShooting();
            }
    }

    // Resets the shooting state and clears the current target.
    protected virtual void StopShooting()
    {
        StopCoroutine(shoot);
        shooting = false;
        targetCollider = null;
        shoot = null;
    }
}