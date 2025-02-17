using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttackSkill", menuName = "Skills/Basic Attack")]
public class BasicAttackSkill : Skill
{
    public override void Execute(GameObject user, GameObject target)
    {
        Debug.Log("Executing BasicAttackSkill...");
        
        if (target == null)
        {
            Debug.LogWarning("BasicAttackSkill: No target provided.");
            return;
        }
        
        // Check if the target is within the skill's range.
        float distance = Vector2.Distance(user.transform.position, target.transform.position);
        Debug.Log($"BasicAttackSkill: Distance to target = {distance}, required range = {range}");
        
        if (distance > range)
        {
            Debug.LogWarning("BasicAttackSkill: Target is out of range.");
            return;
        }
        
        // Use the CombatService to deal damage.
        if (CombatService.Instance != null)
        {
            CombatService.Instance.DealDamage(user, target, power, this);
            Debug.Log("BasicAttackSkill: Damage dealt.");
        }
        else
        {
            Debug.LogError("BasicAttackSkill: CombatService instance is missing.");
        }
    }
}
