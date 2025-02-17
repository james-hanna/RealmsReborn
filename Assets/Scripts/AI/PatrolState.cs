using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState
{
    private Vector2 patrolTarget;
    private bool isWaiting = false;

    public void Enter(EnemyAI enemy)
    {
        ChooseNewPatrolTarget(enemy);
        isWaiting = false;
        Debug.Log($"{enemy.gameObject.name} entered Patrol state.");
    }

public void Update(EnemyAI enemy)
    {
        // Check for a target (e.g., the player) within detection range.
        if (enemy.Target != null)
        {
            float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.Target.position);
            Enemy enemyStats = enemy.GetComponent<Enemy>();
            if (distanceToTarget <= enemyStats.detectionRange)
            {
                enemy.ChangeState(new ChaseState());
                return;
            }
        }

        if (!isWaiting)
        {
            Enemy enemyStats = enemy.GetComponent<Enemy>();

            // Capture the current position before moving.
            Vector3 currentPos = enemy.transform.position;
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, patrolTarget, enemyStats.patrolSpeed * Time.deltaTime);
            Vector3 newPos = enemy.transform.position;

            // Calculate horizontal movement delta.
            float horizontalDelta = newPos.x - currentPos.x;

            // Get the SpriteFlipper component and update direction.
            SpriteFlipper flipper = enemy.GetComponent<SpriteFlipper>();
            if (flipper != null)
            {
                flipper.UpdateDirection(horizontalDelta);
            }

            // If we have reached the patrol target, wait before picking a new target.
            if (Vector2.Distance(enemy.transform.position, patrolTarget) < 0.1f)
            {
                isWaiting = true;
                float waitTime = Random.Range(enemyStats.idleWaitMin, enemyStats.idleWaitMax);
                enemy.StartCoroutine(WaitAndPickNewTarget(enemy, waitTime));
            }
        }
    }

    public void Exit(EnemyAI enemy)
    {
        Debug.Log($"{enemy.gameObject.name} exiting Patrol state.");
    }

    private void ChooseNewPatrolTarget(EnemyAI enemy)
    {
        Enemy enemyStats = enemy.GetComponent<Enemy>();
        float randomX = Random.Range(enemyStats.patrolMinX, enemyStats.patrolMaxX);
        patrolTarget = new Vector2(randomX, enemy.transform.position.y);
    }

    private IEnumerator WaitAndPickNewTarget(EnemyAI enemy, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ChooseNewPatrolTarget(enemy);
        isWaiting = false;
    }
}
