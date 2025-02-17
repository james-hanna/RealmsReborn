using UnityEngine;

public class AttackState : IEnemyState
{
    private float lastAttackTime = 0f;

    public void Enter(EnemyAI enemy)
    {
        lastAttackTime = Time.time;
        Debug.Log($"{enemy.gameObject.name} entered Attack state.");
    }

    public void Update(EnemyAI enemy)
    {
        if (enemy.Target == null)
        {
            enemy.ChangeState(new PatrolState());
            return;
        }

        Enemy enemyStats = enemy.GetComponent<Enemy>();
        float distance = Vector2.Distance(enemy.transform.position, enemy.Target.position);

        // If target is out of attack range, switch back to Chase.
        if (distance > enemyStats.attackRange)
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        // Attack only if the cooldown has passed.
        if (Time.time - lastAttackTime >= enemyStats.attackCooldown)
        {
            if (Vector2.Distance(enemy.transform.position, enemy.Target.position) <= enemyStats.attackRange)
            {
                // Use the CombatService to apply damage.
                CombatService.Instance.DealDamage(enemy.gameObject, enemy.Target.gameObject, enemyStats.attackDamage);
            }
            lastAttackTime = Time.time;
        }
    }

    public void Exit(EnemyAI enemy)
    {
        Debug.Log($"{enemy.gameObject.name} exiting Attack state.");
    }
}
