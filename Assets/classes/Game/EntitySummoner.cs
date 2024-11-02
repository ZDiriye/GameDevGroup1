using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{
    public static List<Enemy> EnemiesInGame;
    public static List<Transform> EnemiesInGameTransform;
    public static Dictionary<int, GameObject> EnemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;

    private static bool isInitialised;

    public static void Init()
    {   
        if (!isInitialised) 
        {
            EnemyPrefabs = new Dictionary<int, GameObject>();
            EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();
            EnemiesInGame = new List<Enemy>();

            EnemySummonData[] Enemies = Resources.LoadAll<EnemySummonData> ("Enemies");
        
            foreach(EnemySummonData enemy in Enemies) 
            {
                EnemyPrefabs.Add(enemy.EnemyID, enemy.EnemyPrefab);
                EnemyObjectPools.Add(enemy.EnemyID, new Queue<Enemy>());
                EnemiesInGameTransform = new List<Transform>();
            }

            isInitialised = true;
        }
        else 
        {
            Debug.Log("Already intialised enitiy summoner");
        }
    }

    public static Enemy SummonEnemy(int EnemyID)
    {
        Enemy SummonedEnemy = null;

        if (EnemyPrefabs.ContainsKey(EnemyID))
        {
            Queue<Enemy> ReferencedQueue = EnemyObjectPools[EnemyID];
            if (ReferencedQueue.Count > 0)
            {
                // Dequeue Enemy + Init
                SummonedEnemy = ReferencedQueue.Dequeue();
                SummonedEnemy.Init();

                SummonedEnemy.gameObject.SetActive(true);
            }
            else 
            {
                GameObject NewEnemy = Instantiate(EnemyPrefabs[EnemyID], GameLoopManager.NodePositions[0], Quaternion.identity);
                SummonedEnemy = NewEnemy.GetComponent<Enemy>();
                SummonedEnemy.Init();
            }
        }
        else 
        {
            Debug.Log("Enemy ID does not exist");
            return null;
        }

        EnemiesInGameTransform.Add(SummonedEnemy.transform);
        EnemiesInGame.Add(SummonedEnemy);
        SummonedEnemy.ID = EnemyID;
        return SummonedEnemy;
    }

    public static void RemoveEnemy(Enemy EnemyToRemove)
    {
        // Reset the health of the enemy before pooling it
        EnemyToRemove.Health = 100f;
        EnemyToRemove.NodeIndex = 0;

        // Enqueue the enemy back to the object pool
        EnemyObjectPools[EnemyToRemove.ID].Enqueue(EnemyToRemove);

        // Deactivate the enemy so it won't be seen until reused
        EnemyToRemove.gameObject.SetActive(false);

        // Remove the enemy from the active enemies list
        EnemiesInGameTransform.Remove(EnemyToRemove.transform);
        EnemiesInGame.Remove(EnemyToRemove);
    }

    public static void Reset()
    {
        // Clear all enemy data and object pools
        EnemiesInGame.Clear();
        EnemiesInGameTransform.Clear();

        foreach (var queue in EnemyObjectPools.Values)
        {
            queue.Clear();
        }

        isInitialised = false;
    }



}
