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
        // Check for a target within detection range.
        if (enemy.Target != null)
        {
            float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.Target.position);
            Enemy enemySettings = enemy.GetComponent<Enemy>();
            if (distanceToTarget <= enemySettings.detectionRange)
            {
                enemy.ChangeState(new ChaseState());
                return;
            }
        }

        if (!isWaiting)
        {
            Enemy enemySettings = enemy.GetComponent<Enemy>();

            // Move horizontally toward patrolTarget (keep y fixed)
            Vector3 currentPos = enemy.transform.position;
            // Ensure we only update the x position; y remains the designated patrol level.
            Vector2 newPos = Vector2.MoveTowards(new Vector2(currentPos.x, enemySettings.patrolY), patrolTarget, enemySettings.patrolSpeed * Time.deltaTime);
            enemy.transform.position = new Vector3(newPos.x, enemySettings.patrolY, currentPos.z);

            // Update sprite flipping.
            float horizontalDelta = newPos.x - currentPos.x;
            SpriteFlipper flipper = enemy.GetComponent<SpriteFlipper>();
            if (flipper != null)
            {
                flipper.UpdateDirection(horizontalDelta);
            }

            if (Vector2.Distance(newPos, patrolTarget) < 0.1f)
            {
                isWaiting = true;
                float waitTime = Random.Range(enemySettings.idleWaitMin, enemySettings.idleWaitMax);
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
        Enemy enemySettings = enemy.GetComponent<Enemy>();
        float randomX = Random.Range(enemySettings.patrolMinX, enemySettings.patrolMaxX);
        // Use the patrolY value from enemySettings
        patrolTarget = new Vector2(randomX, enemySettings.patrolY);
        Debug.Log($"{enemy.gameObject.name} new patrol target set to: {patrolTarget}");
    }

    private IEnumerator WaitAndPickNewTarget(EnemyAI enemy, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ChooseNewPatrolTarget(enemy);
        isWaiting = false;
    }
}
