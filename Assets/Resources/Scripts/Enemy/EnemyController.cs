using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Controls enemy navigation and attacking behaviors.
public class EnemyController : MonoBehaviour
{
    private EnemyManager enemyManager;
    public Animator animator;
    private Transform[] waypoints;
    private NavMeshAgent navMeshAgent;
    private bool attack = false;
    public int maxHealth;
    public int health;
    public Healthbar healthbar;
    public float remainingSlowTime = 0;
    private Coroutine slowed;
    private bool isDead = false;
    private bool hasReportedDeath = false;
 

    // Sets up enemy's navigation agent and starts the movement through waypoints
    public void Initialise(Transform[] waypoints, bool strong, bool fast)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null) 
        {
            Debug.LogError("NavMeshAgent component not found on the GameObject.");
            return;
        }
        
        if (fast && !strong)
        {
            fastPace();
        }

        if (strong)
        {
            maxHealth *= 2;
            transform.localScale *= 1.4f;
            slowPace();
            
        }

        health = maxHealth;
        healthbar.UpdateHealthbar(maxHealth, health);

        this.waypoints = waypoints;
        StartCoroutine(MoveThroughWaypoints(waypoints));
    }

    // moves the enemy through assigned waypoints and triggers attack at the end.
    private IEnumerator MoveThroughWaypoints(Transform[] waypoints)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        int currentWaypoint = 0;
        Debug.Log($"Starting movement through waypoints. Total waypoints: {waypoints.Length}");
        while (currentWaypoint < waypoints.Length)
        {
            // Stop if the agent is missing, disabled, no longer on a NavMesh, or if we're dead.
            if (navMeshAgent == null 
                || !navMeshAgent.isActiveAndEnabled 
                || !navMeshAgent.isOnNavMesh 
                || isDead)
            {
                yield break;
            }

            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
            Debug.Log($"Moving to waypoint {currentWaypoint}: {navMeshAgent.SetDestination(waypoints[currentWaypoint].position)}");

            // Wait until we reach the waypoint or any condition invalidates the agent again
            yield return new WaitUntil(() => 
                navMeshAgent != null 
                && navMeshAgent.isActiveAndEnabled 
                && navMeshAgent.isOnNavMesh 
                && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                && !navMeshAgent.pathPending);

            // Stop if something changed while waiting
            if (isDead 
                || navMeshAgent == null 
                || !navMeshAgent.isActiveAndEnabled 
                || !navMeshAgent.isOnNavMesh)
            {
                yield break;
            }

            Debug.Log($"Reached waypoint {currentWaypoint}");
            currentWaypoint++;
        }
        
        Attack();
    }

    public void Slow(float duration)
    {
        if (remainingSlowTime <= 0) 
        {
            slowPace();
        }
        remainingSlowTime = duration;
    }
 
    // does the attacking animation by setting the attack state.
    private void Attack()
    {
        transform.LookAt(EnemyManager.instance.level.headquaters.transform.position);
        Debug.Log($"Attack");
        attack = true;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Taking {damage} damage, current health: {health}");
        health -= damage;
        healthbar.UpdateHealthbar(maxHealth, health);
        if (health > 0)
        {
            animator.SetBool("IsHit", true);
            Debug.Log("GetHit animation triggered.");

            StartCoroutine(ResetHitAnimation());
        }
        else
        {
            // If health is zero or less, the enemy dies
            Die();
        }
    }   

    private IEnumerator ResetHitAnimation()
    {
        yield return new WaitForSeconds(0.2f); 
        animator.SetBool("IsHit", false);
    }

    private void Die()
    {
        if (!hasReportedDeath)
        {
            Debug.Log("Die Trigger Sent for " + gameObject.name);
            isDead = true;

            if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.enabled = false;
            }

            animator.SetTrigger("Die");
            StartCoroutine(WaitForDeathAnimation());
            hasReportedDeath = true;
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        Debug.Log("Waiting for death animation to complete.");
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        
        EnemyManager.instance.ReportEnemyDeath();

        Debug.Log("Death animation complete, destroying object.");
        Destroy(gameObject);
    }

    private void Update()
    {
        if (remainingSlowTime > 0)
        {
            remainingSlowTime -= Time.deltaTime;
            if (remainingSlowTime <= 0)
            {
                fastPace();
            }
        }

        if (attack)
        {
            transform.LookAt(EnemyManager.instance.level.headquaters.transform.position);
            animator.SetTrigger("Attack");
        }
    }

    private void fastPace()
    {
        navMeshAgent.acceleration *= 2.0f;
        navMeshAgent.speed *= 2.0f;
        navMeshAgent.angularSpeed *= 2.0f;
    }

    private void slowPace()
    {
        navMeshAgent.acceleration *= 0.5f;
        navMeshAgent.speed *= 0.5f;
        navMeshAgent.angularSpeed *= 0.5f;
    }
}
