using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public LevelController level;
    public Transform[] navigationPath0, navigationPath1;
    public static EnemyManager instance;
    public GameObject[] enemyPrefabs;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("More than one EnemyManager in the scene");
        }
    }
    
    private void start()
    {

    }
}
