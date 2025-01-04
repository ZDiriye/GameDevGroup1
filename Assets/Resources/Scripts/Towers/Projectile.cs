using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public Transform Target;
    public float SpeedTravel;
    public ProjectileType projectileType;
    
    private bool initialise = false;
    private float StartTime;
    private Vector3 startPos, targetPosition;

    [SerializeField] private float arcHeight;       
    [SerializeField] private float distanceThreshold;
    [SerializeField] private GameObject explosionEffect; 
    private int damage;
    private int aoe;
    private float percentage;

    // Called by the tower. This is how we know the final position we want to reach.
    public void Initialise(Vector3 target, float speedTravel, int damage, int aoe, float percentage) //damage
    {
        this.targetPosition = target;
        this.SpeedTravel = speedTravel;
        this.StartTime = Time.time;
        this.damage = damage;
        this.aoe = aoe;

        if (projectileType == ProjectileType.IceBall)
        {
            this.percentage = percentage;
        }
        initialise = true;
    }

    public void Initialise(Transform target, float speedTravel, int damage) //damage
    {
        this.Target = target;
        this.SpeedTravel = speedTravel;
        this.StartTime = Time.time;
        this.damage = damage;
        initialise = true;

        if (Target != null)
        {
            Vector3 direction = (Target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (!initialise) return;

        if (projectileType == ProjectileType.Arrow)
        {
            if (Target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 direction = Target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1000000);

            transform.position = Vector3.MoveTowards(transform.position, Target.position, 45 * SpeedTravel * Time.deltaTime);
        }
        else if (projectileType == ProjectileType.CannonBall || projectileType == ProjectileType.IceBall)
        {
            if (targetPosition == null)
            {
                Destroy(gameObject);
                return;
            }

            float totalDistance = Vector3.Distance(startPos, targetPosition);
            float movePerSec = 35f * SpeedTravel;
            float distanceCovered = movePerSec * (Time.time - StartTime);
            float t = distanceCovered / totalDistance;

            // Clamp t to [0..1] so we don't overshoot
            t = Mathf.Clamp01(t);

            // Horizontal interpolation from startPos to targetPos
            Vector3 horizontalPos = Vector3.Lerp(startPos, targetPosition, t);

            // Add a vertical arc using a sine wave
            float offset = Mathf.Sin(Mathf.PI * t) * arcHeight;

            // Final position
            transform.position = new Vector3(
                horizontalPos.x,
                horizontalPos.y + offset,
                horizontalPos.z
            );

            float currentDistanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (currentDistanceToTarget < distanceThreshold)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        AOE(aoe);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && projectileType == ProjectileType.Arrow)
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void AOE(float size)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, size);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (projectileType == ProjectileType.CannonBall)
                {
                    hit.GetComponent<EnemyController>().TakeDamage(damage); //damage
                }
                else if (projectileType == ProjectileType.IceBall)
                {
                    hit.GetComponent<EnemyController>().TakeDamage(damage); //damage
                    hit.GetComponent<EnemyController>().Slow(5, percentage);
                }
            }
        }
        Destroy(gameObject);
    }
}