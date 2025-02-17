using UnityEngine;

public enum BuffType
{
    IncreaseDamage,
    IncreaseDefense,
    IncreaseSpeed
    // Add more buff types as needed.
}

[CreateAssetMenu(fileName = "NewBuffSkill", menuName = "Skills/Buff Skill")]
public class BuffSkill : Skill
{
    [Header("Buff Settings")]
    [Tooltip("Type of buff to apply.")]
    public BuffType buffType;
    [Tooltip("Magnitude of the buff (could be a percentage or a flat value).")]
    public float buffAmount;
    [Tooltip("Duration of the buff (in seconds).")]
    public float buffDuration;

    public override void Execute(GameObject user, GameObject target)
    {
        Buff buff = user.GetComponent<Buff>();
        if (buff == null)
        {
            buff = user.AddComponent<Buff>();
        }
        buff.ApplyBuff(buffType, buffAmount, buffDuration);
        Debug.Log($"{user.name} received a {buffType} buff of {buffAmount} for {buffDuration} seconds.");

        // Optionally, display floating text
        if (DamageTextSpawner.Instance != null)
        {
            DamageTextSpawner.Instance.ShowHealText(user.transform.position, Mathf.RoundToInt(buffAmount));
        }
    }
}
