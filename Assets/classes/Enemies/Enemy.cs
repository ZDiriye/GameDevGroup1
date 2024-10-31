using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int NodeIndex;
    public float Speed;
    public int ID;
    public float Health;

    public void Init() 
    {
        // Start the enemy at the first node position
        transform.position = GameLoopManager.NodePositions[0];
        NodeIndex = 0;
        Health = 100f;
    }

    // For Enemy to take damage
    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        Debug.Log($"Enemy ID: {ID} took damage: {damageAmount}, Health remaining: {Health}");
        // Check if health is less than or equal to 0, if so, enemy dies
        if (Health <= 0)
        {
            Die();
        }
    }

    // To remove the enemy once it's damage reaches 0
    private void Die()
    {
        EntitySummoner.RemoveEnemy(this);
    }

}
