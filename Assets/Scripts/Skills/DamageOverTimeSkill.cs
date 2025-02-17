using UnityEngine;

[CreateAssetMenu(fileName = "NewDOTSkill", menuName = "Skills/Damage Over Time Skill")]
public class DamageOverTimeSkill : Skill
{
    [Header("DOT Settings")]
    [Tooltip("Damage applied at each tick.")]
    public int tickDamage;
    [Tooltip("Time interval (in seconds) between ticks.")]
    public float tickInterval;
    [Tooltip("Total duration of the DOT effect.")]
    public float dotDuration;
    [Tooltip("If true, the skill applies DOT as an AoE effect.")]
    public bool isAOE;

    public override void Execute(GameObject user, GameObject target)
    {
        if (isAOE)
        {
            // For AoE, use the target's position if provided, otherwise use the user's position.
            Vector2 center = target != null ? (Vector2)target.transform.position : (Vector2)user.transform.position;
            // Use the inherited aoeRadius property.
            int enemyLayer = LayerMask.GetMask("Enemy");
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, aoeRadius, enemyLayer);
            Debug.Log($"DOTSkill (AoE): Found {hits.Length} targets at {center}.");
            foreach (Collider2D hit in hits)
            {
                CombatService.Instance.ApplyDamageOverTime(hit.gameObject, tickDamage, tickInterval, dotDuration);
            }
        }
        else
        {
            if (target == null)
            {
                Debug.Log("DOTSkill: No target provided for single-target DOT.");
                return;
            }
            float distance = Vector2.Distance(user.transform.position, target.transform.position);
            if (distance > range)
            {
                Debug.Log("DOTSkill: Target is out of range.");
                return;
            }
            CombatService.Instance.ApplyDamageOverTime(target, tickDamage, tickInterval, dotDuration);
        }
    }
}
