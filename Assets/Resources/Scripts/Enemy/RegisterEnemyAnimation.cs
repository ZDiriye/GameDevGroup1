using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterEnemyAnimation : MonoBehaviour
{
    public float damage;

    public void DealDamageToHeadquarters()
    {
        EnemyManager.instance.level.headquaters.Damage(damage);
    }
}
