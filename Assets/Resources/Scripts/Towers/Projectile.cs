using System.Collections;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public Transform Target;
    public Vector3 StartMarker;
    public float SpeedTravel;
    public bool initialise = false;
    public float StartTime;
 
    // Initializes the projectile with a target and movement speed.
    public void Initialise(Transform target, float speedTravel)
    {
        this.Target = target;
        this.SpeedTravel = speedTravel;
        StartTime = Time.time;
        initialise = true;
    }

    // Moves the projectile toward the target and checks for collisions.
    private void Update()
    {
        if (!initialise || Target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.LookAt(Target);
        // Move toward the target.
        transform.position = Vector3.MoveTowards(transform.position, Target.position, 50 * SpeedTravel * Time.deltaTime);

        // Check if the projectile has reached the target.
        if (Vector3.Distance(transform.position, Target.position) < 0.1f)
        {
            OnHitTarget();
        }
    }

    // Handles the event when the projectile hits its target.
    protected virtual void OnHitTarget()
    {
        if (Target != null && Target.CompareTag("Enemy"))
        {
            Target.GetComponent<EnemyController>()?.TakeDamage(20);
        }

        Destroy(gameObject);
    }

    // Registers a collision and applies damage if the target is an enemy.
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>()?.TakeDamage(10);
            Destroy(gameObject);
        }
    }
}
