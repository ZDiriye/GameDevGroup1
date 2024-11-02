using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int NodeIndex;
    public float Speed;
    public int ID;
    public float Health;
    public bool IsTargeted; //used to prevent towers shooting at the same enemy

    public void Init() 
    {
        // Start the enemy at the first node position
        transform.position = GameLoopManager.NodePositions[0];
        NodeIndex = 0;
        Health = 100f;
        IsTargeted = false; // set to not targeted first
    }

    // For Enemy to take damage
    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        Debug.Log($"Enemy ID: {ID} took damage: {damageAmount}, Health remaining: {Health}");
        
        if (Health <= 0)
        {
            Die();
        }
        else
        {
            // enemy can be targeted since its still alive
            IsTargeted = false;
        }
    }

    private void Die()
    {
        // enemy can be targeted since its still alive
        IsTargeted = false;
        EntitySummoner.RemoveEnemy(this);
    }
}
