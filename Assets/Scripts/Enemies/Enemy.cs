using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : CommonStats
{
    [Header("Identification & Rewards")]
    public string enemyName = "Enemy";
    public int xpReward = 50;

    [Header("Patrol Settings (X Axis)")]
    [Tooltip("Left boundary of the patrol area.")]
    public float patrolMinX = -5f;
    [Tooltip("Right boundary of the patrol area.")]
    public float patrolMaxX = 5f;
    [Tooltip("Speed while patrolling.")]
    public float patrolSpeed = 2f;
    [Tooltip("Minimum idle time when reaching a patrol point.")]
    public float idleWaitMin = 0.5f;
    [Tooltip("Maximum idle time when reaching a patrol point.")]
    public float idleWaitMax = 2f;

    [Header("Detection & Chase Settings")]
    [Tooltip("Distance at which the enemy detects the player.")]
    public float detectionRange = 8f;
    [Tooltip("Speed while chasing the player.")]
    public float chaseSpeed = 3f;

    [Header("Attack Settings")]
    [Tooltip("Distance within which the enemy will attack.")]
    public float attackRange = 2f;
    [Tooltip("Cooldown (in seconds) between attacks.")]
    public float attackCooldown = 1f;
    [Tooltip("Damage per attack.")]
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
