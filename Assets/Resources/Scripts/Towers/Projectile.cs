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

    // Called by the tower. This is how we know the final position we want to reach.
    public void Initialise(Vector3 target, float speedTravel)
    {
        this.targetPosition = target;
        this.SpeedTravel = speedTravel;
        this.StartTime = Time.time;
        initialise = true;
    }

    public void Initialise(Transform target, float speedTravel)
    {
        this.Target = target;
        this.SpeedTravel = speedTravel;
        this.StartTime = Time.time;
        initialise = true;
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (projectileType == ProjectileType.Arrow)
        {
            if (Target == null)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, Target.position, 50 * SpeedTravel * Time.deltaTime);
            transform.LookAt(Target);
        }
        // else if (projectileType == ProjectileType.CannonBall)
        // {
        //     if (targetPosition == null)
        //     {
        //         Destroy(gameObject);
        //         return;
        //     }

        //     float totalDistance = Vector3.Distance(startPos, targetPosition);
        //     float movePerSec = 15f * SpeedTravel;
        //     float distanceCovered = movePerSec * (Time.time - StartTime);
        //     float t = distanceCovered / totalDistance;

        //     // Clamp t to [0..1] so we don't overshoot
        //     t = Mathf.Clamp01(t);

        //     // Horizontal interpolation from startPos to targetPos
        //     Vector3 horizontalPos = Vector3.Lerp(startPos, targetPosition, t);

        //     // Add a vertical arc using a sine wave
        //     float offset = Mathf.Sin(Mathf.PI * t) * arcHeight;

        //     // Final position
        //     transform.position = new Vector3(
        //         horizontalPos.x,
        //         horizontalPos.y + offset,
        //         horizontalPos.z
        //     );

        //     float currentDistanceToTarget = Vector3.Distance(transform.position, targetPosition);
        //     if (currentDistanceToTarget < distanceThreshold)
        //     {
        //         AOE(15);
        //     }
        // }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && projectileType == ProjectileType.Arrow)
        {
            other.GetComponent<EnemyController>().TakeDamage(10);
            Destroy(gameObject);
        }
    }

    // private void AOE(float size)
    // {
    //     Collider[] hits = Physics.OverlapSphere(transform.position, size);
    //     foreach (Collider hit in hits)
    //     {
    //         if (hit.CompareTag("Enemy"))
    //         {
    //             hit.GetComponent<EnemyController>().TakeDamage(20);
    //         }
    //     }
    //     Destroy(gameObject);
    // }
}