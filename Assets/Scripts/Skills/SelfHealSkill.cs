using UnityEngine;

[CreateAssetMenu(fileName = "SelfHealSkill", menuName = "Skills/Self Heal")]
public class SelfHealSkill : Skill
{
    public override void Execute(GameObject user, GameObject target)
    {
        // For a self-heal, we ignore the target parameter and use the user.
        CommonStats stats = user.GetComponent<CommonStats>();
        if (stats == null)
        {
            Debug.Log("SelfHealSkill: No CommonStats found on user.");
            return;
        }
        stats.Heal(power);
        Debug.Log($"{user.name} healed for {power} HP.");

        if (DamageTextSpawner.Instance != null)
        {
            DamageTextSpawner.Instance.ShowHealText(user.transform.position, power);
        }
    }
}
