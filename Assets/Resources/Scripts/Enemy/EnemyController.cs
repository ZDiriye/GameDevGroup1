using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Controls enemy navigation and attacking behaviors.
public class EnemyController : MonoBehaviour
{
    private EnemyManager enemyManager; // Reference to the EnemyManager
    public Animator animator; // Reference to the Animator for animations
    private Transform[] waypoints; // List of waypoints for the enemy to navigate through
    private NavMeshAgent navMeshAgent; // NavMeshAgent for pathfinding
    private bool attack = false; // Tracks whether the enemy is in attack mode
    public int maxHealth; // Maximum health of the enemy
    public int health; // Current health of the enemy
    public Healthbar healthbar; // Reference to the health bar UI
    public float remainingSlowTime = 0; // Remaining time for the slow effect
    private Coroutine slowed; // Reference to the slow effect coroutine
    private bool isDead = false; // Tracks whether the enemy is dead
    private bool hasReportedDeath = false; // Ensures death is only reported once
    public int killReward; // Reward for killing this enemy

    // Initializes the enemy's properties, sets navigation, and starts movement
    public void Initialise(Transform[] waypoints, bool strong, bool fast)
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get NavMeshAgent component
        if (navMeshAgent == null) 
        {
            Debug.LogError("NavMeshAgent component not found on the GameObject.");
            return;
        }
        
        // If the enemy is fast but not strong, adjust properties
        if (fast && !strong)
        {
            killReward = 20; // Set reward for fast enemy
            fastPace(); // Increase movement speed
        }

        // If the enemy is strong, adjust properties
        if (strong)
        {
            maxHealth *= 2; // Double the health
            killReward = 25; // Set reward for strong enemy
            transform.localScale *= 1.2f; // Scale up the enemy
            slowPace(); // Reduce movement speed
        }

        health = maxHealth; // Set current health to max health
        healthbar.UpdateHealthbar(maxHealth, health); // Initialize health bar

        this.waypoints = waypoints; // Assign waypoints for navigation
        StartCoroutine(MoveThroughWaypoints(waypoints)); // Start navigation
    }

    // Moves the enemy through assigned waypoints and triggers attack at the end
    private IEnumerator MoveThroughWaypoints(Transform[] waypoints)
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get NavMeshAgent component
        int currentWaypoint = 0;
        Debug.Log($"Starting movement through waypoints. Total waypoints: {waypoints.Length}");

        while (currentWaypoint < waypoints.Length)
        {
            // Stop if the agent is invalid or if the enemy is dead
            if (navMeshAgent == null 
                || !navMeshAgent.isActiveAndEnabled 
                || !navMeshAgent.isOnNavMesh 
                || isDead)
            {
                yield break;
            }

            // Set destination to the next waypoint
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
            Debug.Log($"Moving to waypoint {currentWaypoint}: {navMeshAgent.SetDestination(waypoints[currentWaypoint].position)}");

            // Wait until the destination is reached or the agent becomes invalid
            yield return new WaitUntil(() => 
                navMeshAgent != null 
                && navMeshAgent.isActiveAndEnabled 
                && navMeshAgent.isOnNavMesh 
                && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                && !navMeshAgent.pathPending);

            // Stop if the agent becomes invalid or the enemy is dead
            if (isDead 
                || navMeshAgent == null 
                || !navMeshAgent.isActiveAndEnabled 
                || !navMeshAgent.isOnNavMesh)
            {
                yield break;
            }

            Debug.Log($"Reached waypoint {currentWaypoint}");
            currentWaypoint++; // Move to the next waypoint
        }
        
        Attack(); // Trigger attack once all waypoints are reached
    }

    // Applies a slow effect to the enemy for a given duration
    public void Slow(float duration, float percentage)
    {
        if (remainingSlowTime <= 0) 
        {
            navMeshAgent.acceleration *= percentage;
            navMeshAgent.speed *= percentage;
            navMeshAgent.angularSpeed *= percentage;
        }
        remainingSlowTime = duration; // Set the slow effect duration
    }
 
    // Triggers the attack animation and sets the enemy to attack mode
    private void Attack()
    {
        transform.LookAt(EnemyManager.instance.level.headquaters.transform.position); // Face the target
        Debug.Log($"Attack");
        attack = true; // Enable attack mode
    }

    // Handles damage taken by the enemy
    public void TakeDamage(int damage)
    {
        Debug.Log($"Taking {damage} damage, current health: {health}");
        health -= damage; // Reduce health
        healthbar.UpdateHealthbar(maxHealth, health); // Update the health bar

        if (health > 0)
        {
            animator.SetBool("IsHit", true); // Trigger hit animation
            Debug.Log("GetHit animation triggered.");

            StartCoroutine(ResetHitAnimation()); // Reset hit animation after a delay
        }
        else
        {   
            Die(); // Trigger death if health is 0 or below
        }
    }   

    // Resets the hit animation state
    private IEnumerator ResetHitAnimation()
    {
        yield return new WaitForSeconds(0.2f); 
        animator.SetBool("IsHit", false);
    }

    // Handles enemy death logic
    private void Die()
    {
        if (!hasReportedDeath)
        {
            Debug.Log("Die Trigger Sent for " + gameObject.name);
            isDead = true; // Mark the enemy as dead

            if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.isStopped = true; // Stop the agent
                navMeshAgent.enabled = false; // Disable the agent
            }

            animator.SetTrigger("Die"); // Trigger death animation
            StartCoroutine(WaitForDeathAnimation()); // Wait for death animation to complete
            hasReportedDeath = true; // Ensure death is reported only once
        }
    }

    // Waits for the death animation to finish before destroying the enemy
    private IEnumerator WaitForDeathAnimation()
    {
        EconomyManager.Instance.AddCurrency(killReward); // Reward player for killing the enemy
        Debug.Log("Waiting for death animation to complete.");
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        EnemyManager.instance.ReportEnemyDeath(); // Notify the EnemyManager of the death

        Debug.Log("Death animation complete, destroying object.");
        Destroy(gameObject); // Destroy the enemy GameObject
    }

    private void Update()
    {
        // Reduce the remaining slow effect duration
        if (remainingSlowTime > 0)
        {
            remainingSlowTime -= Time.deltaTime;
            if (remainingSlowTime <= 0)
            {
                fastPace(); // Reset to normal speed
            }
        }

        // Trigger attack animation if in attack mode
        if (attack)
        {
            transform.LookAt(EnemyManager.instance.level.headquaters.transform.position); // Face the target
            animator.SetTrigger("Attack"); // Trigger attack animation
        }
    }

    // Increases the enemy's movement speed
    private void fastPace()
    {
        navMeshAgent.acceleration *= 2.0f;
        navMeshAgent.speed *= 2.0f;
        navMeshAgent.angularSpeed *= 2.0f;
    }

    // Reduces the enemy's movement speed
    private void slowPace()
    {
        navMeshAgent.acceleration *= 0.5f;
        navMeshAgent.speed *= 0.5f;
        navMeshAgent.angularSpeed *= 0.5f;
    }
}
