using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("Prefab for a normal enemy.")]
    public GameObject enemyPrefab;
    [Tooltip("Prefab for an elite enemy.")]
    public GameObject eliteEnemyPrefab;

    [Header("Spawn Settings")]
    [Tooltip("Total number of enemies to maintain.")]
    public int spawnCount = 5;
    [Tooltip("Maximum distance (on x-axis) from the spawner for spawn locations.")]
    public float spawnRadius = 10f;
    [Tooltip("Delay in seconds before respawning an enemy after one dies.")]
    public float respawnDelay = 5f;
    [Range(0f, 1f)]
    [Tooltip("Chance to spawn an elite enemy instead of a normal one.")]
    public float eliteChance = 0.1f;

    private int currentEnemyCount = 0;

    private void Start()
    {
        // Spawn the initial batch of enemies.
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
        }
    }


    /// Called by an enemy when it dies.
    public void OnEnemyDeath()
    {
        currentEnemyCount--;
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        Vector3 basePosition = transform.position;
        // Random x offset within [-spawnRadius, spawnRadius].
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        Vector3 spawnPosition = basePosition + new Vector3(randomX, 0f, 0f);

        // Determine whether to spawn an elite enemy.
        GameObject prefabToSpawn = (Random.value <= eliteChance && eliteEnemyPrefab != null) ? eliteEnemyPrefab : enemyPrefab;
        if (prefabToSpawn == null)
        {
            Debug.LogError("EnemySpawner: No valid prefab assigned for spawning.");
            return;
        }

        // Instantiate the enemy.
        GameObject newEnemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        // Set the spawner reference on the enemy.
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.spawner = this;
        }
        else
        {
            Debug.LogWarning("EnemySpawner: Spawned enemy does not have an Enemy component.");
        }
        currentEnemyCount++;
    }
}
