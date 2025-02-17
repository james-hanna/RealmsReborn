using UnityEngine;

public class ChaseState : IEnemyState
{
    public void Enter(EnemyAI enemy)
    {
        Debug.Log($"{enemy.gameObject.name} entered Chase state.");
    }

    public void Update(EnemyAI enemy)
    {
        if (enemy.Target == null)
        {
            enemy.ChangeState(new PatrolState());
            return;
        }

        Enemy enemyStats = enemy.GetComponent<Enemy>();

        // Capture the enemy's current position
        Vector2 currentPos = enemy.transform.position;
        Vector2 targetPos = new Vector2(enemy.Target.position.x, currentPos.y);

        // Move the enemy toward the target position.
        enemy.transform.position = Vector2.MoveTowards(currentPos, targetPos, enemyStats.chaseSpeed * Time.deltaTime);

        // Calculate movement delta for sprite flipping.
        float horizontalDelta = enemy.transform.position.x - currentPos.x;
        SpriteFlipper flipper = enemy.GetComponent<SpriteFlipper>();
        if (flipper != null)
        {
            flipper.UpdateDirection(horizontalDelta);
        }

        // Use horizontal distance for transitioning to attack.
        float horizontalDistance = Mathf.Abs(enemy.transform.position.x - enemy.Target.position.x);
        if (horizontalDistance <= enemyStats.attackRange)
        {
            enemy.ChangeState(new AttackState());
        }
        // If the target moves outside the patrol area, revert to Patrol.
        else if (enemy.Target.position.x < enemyStats.patrolMinX || enemy.Target.position.x > enemyStats.patrolMaxX)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit(EnemyAI enemy)
    {
        Debug.Log($"{enemy.gameObject.name} exiting Chase state.");
    }
}
