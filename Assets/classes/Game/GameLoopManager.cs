using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    private float timer; 
    private const float gameDuration = 30f;

    public static Vector3[] NodePositions;
    private static Queue<Enemy> EnemiesToRemove;
    private static Queue<int> EnemyIDsToSummon;

    public Transform NodeParent;
    public bool LoopShouldEnd;
    public static bool GameIsActive = true;
    public GameObject placeTowerButton;

    public GameObject gameOverText;  
    public GameObject PlayAgainButton;  
    public GameObject BackToMainMenuButton; 

    private int enemiesRemovedCount = 0;
    private const int maxEnemiesAllowed = 5;

    // Timer to control enemy spawns
    public float spawnInterval = 20f; // Adjust this value to increase or decrease the spawn delay
    private float spawnTimer = 0f; 
    

    void Start()
    {
        Debug.Log("GameLoopManager Start() called");
        EnemyIDsToSummon = new Queue<int>();
        EnemiesToRemove = new Queue<Enemy>();
        EntitySummoner.Init();

        NodePositions = new Vector3[NodeParent.childCount];
        for(int i = 0; i < NodePositions.Length; i++)
        {
            NodePositions[i] = NodeParent.GetChild(i).position;
        }

        StartCoroutine(GameLoop());
        InvokeRepeating("SummonTest", 0f, 1f);
        gameOverText.SetActive(false);
        PlayAgainButton.SetActive(false);
        BackToMainMenuButton.SetActive(false);
        GameIsActive = true;
        placeTowerButton.SetActive(true);
    }

    void SummonTest ()
    {
        Debug.Log("SummonTest: Attempting to enqueue enemy ID 1");
        EnqueueEnemyIDToSummon(1);
    }

    IEnumerator GameLoop () 
    {   
        timer = gameDuration;

        while (!LoopShouldEnd) 
        {   
            // Countdown timer
            timer -= Time.deltaTime;

            // Check if time is up => Game Won
            if (timer <= 0)
            {
                TriggerGameWon();
                yield break;
            }

            // Handle enemy spawning based on spawnInterval
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                // Time to spawn a new enemy
                spawnTimer = 0f;
                if (EnemyIDsToSummon.Count > 0) {
                    EntitySummoner.SummonEnemy(EnemyIDsToSummon.Dequeue());
                }
            }

            // Move Enemies (simple loop, no jobs)
            MoveEnemies();

            // Remove enemies if any are queued for removal
            if (EnemiesToRemove.Count > 0)
            {
                for (int i = 0; i < EnemiesToRemove.Count; i++)
                {
                    EntitySummoner.RemoveEnemy(EnemiesToRemove.Dequeue());

                    enemiesRemovedCount++;
                    Debug.Log("Enemy Removed! Count: " + enemiesRemovedCount);

                    if (enemiesRemovedCount >= maxEnemiesAllowed)
                    {
                        TriggerGameOver();
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }

    void MoveEnemies()
    {
        if (EntitySummoner.EnemiesInGame.Count > 0)
        {
            foreach (Enemy enemy in EntitySummoner.EnemiesInGame)
            {
                if (enemy.NodeIndex < NodePositions.Length)
                {
                    Vector3 targetPos = NodePositions[enemy.NodeIndex];
                    Vector3 direction = targetPos - enemy.transform.position;
                    float distanceThisFrame = enemy.Speed * Time.deltaTime;

                    // If close enough to the target node
                    if (direction.magnitude <= distanceThisFrame)
                    {
                        // Snap to the target position
                        enemy.transform.position = targetPos;
                        enemy.NodeIndex++;

                        // Check if the enemy has reached the end
                        if (enemy.NodeIndex >= NodePositions.Length)
                        {
                            // Enemy reached the end, remove it
                            EnqueueEnemyToRemove(enemy);
                        }
                        else
                        {
                            // Face the next node
                            enemy.transform.LookAt(NodePositions[enemy.NodeIndex]);
                        }
                    }
                    else
                    {
                        // Move towards the target
                        enemy.transform.LookAt(targetPos);
                        enemy.transform.Translate(direction.normalized * distanceThisFrame, Space.World);
                    }
                }
            }
        }
    }

    void TriggerGameOver()
    {
        Debug.Log("Game Over!");
        gameOverText.SetActive(true);
        PlayAgainButton.SetActive(true);
        BackToMainMenuButton.SetActive(true);
        placeTowerButton.SetActive(false);

        LoopShouldEnd = true;
        GameIsActive = false;
    }

    void TriggerGameWon()
    {
        Debug.Log("Game Won!");
        gameOverText.SetActive(true); 
        PlayAgainButton.SetActive(true);
        BackToMainMenuButton.SetActive(true);
        placeTowerButton.SetActive(false);

        // Change the text to "GAME WON!"
        UnityEngine.UI.Text uiText = gameOverText.GetComponentInChildren<UnityEngine.UI.Text>();
        if (uiText != null)
        {
            uiText.text = "GAME WON!";
            uiText.color = Color.green;
        }

        LoopShouldEnd = true;
        GameIsActive = false;
    }

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        EnemyIDsToSummon.Enqueue(ID);
    }

    public static void EnqueueEnemyToRemove (Enemy EnemyToRemove)
    {
        EnemiesToRemove.Enqueue(EnemyToRemove);
    }
}
