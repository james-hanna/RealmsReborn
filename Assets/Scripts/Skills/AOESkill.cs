using UnityEngine;

[CreateAssetMenu(fileName = "AOESkill", menuName = "Skills/AOE Attack")]
public class AOESkill : Skill
{
    public override void Execute(GameObject user, GameObject target)
    {
        // Determine center: use the target's position if provided, otherwise use user's position.
        Vector2 center = target != null ? (Vector2)target.transform.position : (Vector2)user.transform.position;
        Debug.Log($"AOESkill: Center = {center}, aoeRadius = {aoeRadius}");
        
        // Ensure that the layer mask matches your enemy layers.
        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, aoeRadius, enemyLayer);
        Debug.Log($"AOESkill: Found {hits.Length} target(s).");
        
        if (hits.Length == 0)
        {
            Debug.LogWarning("AOESkill: No enemies found within AoE range.");
        }
        
        foreach (Collider2D hit in hits)
        {
            GameObject enemyGO = hit.gameObject;
            if (CombatService.Instance != null)
            {
                CombatService.Instance.DealDamage(user, enemyGO, power, this);
                Debug.Log("AOESkill: Damage dealt to " + enemyGO.name);
            }
            else
            {
                Debug.LogError("AOESkill: CombatService instance is missing.");
            }
        }
    }
}
