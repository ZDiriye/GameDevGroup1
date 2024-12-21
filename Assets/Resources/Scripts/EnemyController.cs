using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// controls enemy navigation and attacking behaviors.
public class EnemyController : MonoBehaviour
{
    public Animator animator;
    private Transform[] waypoints;
    private NavMeshAgent navMeshAgent;
    private bool attack = false;

    public void Start() 
    {
        Initialise(EnemyManager.instance.navigationPath0);
    }

    // sets up enemy's navigation agent and starts the movement through waypoints.
    public void Initialise(Transform[] waypoints)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null) 
        {
            Debug.LogError("NavMeshAgent component not found on the GameObject.");
            return;
        }
        Debug.Log(navMeshAgent.isOnNavMesh);
        this.waypoints = waypoints;
        StartCoroutine(MoveThroughWaypoints(waypoints));
    }

    // moves the enemy through assigned waypoints and triggers attack at the end.
    private IEnumerator MoveThroughWaypoints(Transform[] waypoints)
    {
        int currentWaypoint = 0;
        Debug.Log($"Starting movement through waypoints. Total waypoints: {waypoints.Length}");
        while (currentWaypoint < waypoints.Length) 
        {
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position); 
            Debug.Log($"Moving to waypoint {currentWaypoint}: {navMeshAgent.SetDestination(waypoints[currentWaypoint].position)}");
            yield return new WaitUntil(() => navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending);
            Debug.Log($"Reached waypoint {currentWaypoint}");
            currentWaypoint++;
        }
        
        Attack();
    }
 
    // does the attacking animation by setting the attack state.
    private void Attack()
    {
        transform.LookAt(EnemyManager.instance.level.headquaters.transform.position);
        Debug.Log($"Attack");
        attack = true;

    }

    // updates each frame to handle attack animation triggering.
    private void Update()
    {
        if (attack)
        {
            transform.LookAt(EnemyManager.instance.level.headquaters.transform.position);
            animator.SetTrigger("Attack");
        }

    }
}
