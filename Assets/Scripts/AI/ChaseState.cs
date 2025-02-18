using UnityEngine;

public class ChaseState : IEnemyState
{
    public void Enter(EnemyAI enemy)
    {
        Debug.Log($"{enemy.gameObject.name} entered Chase state.");
    }

    public void Update(EnemyAI enemy)
    {
        // If there is no target, return to patrol.
        if (enemy.Target == null)
        {
            enemy.ChangeState(new PatrolState());
            return;
        }

        Enemy enemySettings = enemy.GetComponent<Enemy>();
        if (enemySettings == null)
        {
            Debug.LogWarning($"{enemy.gameObject.name} missing Enemy component.");
            enemy.ChangeState(new PatrolState());
            return;
        }
        
        // Check if the target's x-coordinate is still within the enemy's patrol boundaries.
        float targetX = enemy.Target.position.x;
        if (targetX < enemySettings.patrolMinX || targetX > enemySettings.patrolMaxX)
        {
            Debug.Log($"{enemy.gameObject.name}: Target left patrol area. Reverting to PatrolState.");
            enemy.ChangeState(new PatrolState());
            return;
        }

        // Move horizontally toward the target.
        // We'll force the enemy to patrol on the designated patrolY level.
        Vector2 currentPos = enemy.transform.position;
        Vector2 targetPos = new Vector2(enemy.Target.position.x, enemySettings.patrolY);
        enemy.transform.position = Vector2.MoveTowards(currentPos, targetPos, enemySettings.chaseSpeed * Time.deltaTime);

        // Optionally update sprite direction.
        float horizontalDelta = enemy.transform.position.x - currentPos.x;
        SpriteFlipper flipper = enemy.GetComponent<SpriteFlipper>();
        if (flipper != null)
        {
            flipper.UpdateDirection(horizontalDelta);
        }

        // Transition to AttackState if within attack range.
        float horizontalDistance = Mathf.Abs(enemy.transform.position.x - enemy.Target.position.x);
        if (horizontalDistance <= enemySettings.attackRange)
        {
            enemy.ChangeState(new AttackState());
        }
    }

    public void Exit(EnemyAI enemy)
    {
        Debug.Log($"{enemy.gameObject.name} exiting Chase state.");
    }
}
