using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : CommonStats
{
    [Header("Identification & Rewards")]
    public string enemyName = "Enemy";
    public int xpReward = 50;

    [Header("Patrol Settings (X Axis)")]
    public float patrolMinX = -5f;
    public float patrolMaxX = 5f;
    public float patrolSpeed = 2f;
    public float idleWaitMin = 0.5f;
    public float idleWaitMax = 2f;

    [Header("Patrol Settings (Y Axis)")]
    [Tooltip("The y position that the enemy should patrol at (e.g., the floor level).")]
    public float patrolY = 0f;

    [Header("Detection & Chase Settings")]
    public float detectionRange = 8f;
    public float chaseSpeed = 3f;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int attackDamage = 10;

    // Reference to the spawner that created this enemy.
    [HideInInspector]
    public EnemySpawner spawner;

    protected override void Awake()
    {
        base.Awake(); // Initializes health in CommonStats.
        // If enemyName is not set, default to the GameObject's name.
        if (string.IsNullOrEmpty(enemyName))
        {
            enemyName = gameObject.name;
        }
    }

    protected override void Die()
    {
        Debug.Log($"{enemyName} has died and awards {xpReward} XP.");

        // Award XP to the player.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerStats ps = player.GetComponent<PlayerStats>();
            if (ps != null)
            {
                ps.GainXP(xpReward);
                Debug.Log($"{player.name} now has {ps.currentXP} XP.");
            }
            else
            {
                Debug.LogWarning("Enemy: PlayerStats component not found on player.");
            }
        }
        else
        {
            Debug.LogWarning("Enemy: No player found in the scene.");
        }

        // Notify the spawner that this enemy has died.
        if (spawner != null)
        {
            spawner.OnEnemyDeath();
        }

        // call the base Die() method to handle destruction.
        base.Die();
    }
}
