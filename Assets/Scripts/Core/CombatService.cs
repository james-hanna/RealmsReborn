using System;
using UnityEngine;

public class CombatService : MonoBehaviour
{
    public static CombatService Instance { get; private set; }

    public event Action<DamageInfo> OnDamageDealt;

    // Default critical hit settings; later move these to attacker stats or the skill itself.
    [Header("Critical Hit Defaults")]
    public float defaultCritChance = 0.2f;       // 20% chance
    public float defaultCritMultiplier = 2f;     // 2× damage on crit

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


public void DealDamage(GameObject attacker, GameObject target, int baseDamage, Skill usedSkill = null)
{
    if (target == null) return;

    CommonStats targetStats = target.GetComponent<CommonStats>();
    if (targetStats == null) return;

    // Determine if this hit is critical.
    bool isCritical = UnityEngine.Random.value <= defaultCritChance;
    int finalDamage = isCritical ? Mathf.RoundToInt(baseDamage * defaultCritMultiplier) : baseDamage;

    // Check for an IncreaseDamage buff on the attacker.
    Buff damageBuff = attacker.GetComponent<Buff>();
    if (damageBuff != null && damageBuff.currentBuffType == BuffType.IncreaseDamage)
    {
        finalDamage = Mathf.RoundToInt(finalDamage * (1 + damageBuff.buffAmount));
        Debug.Log($"{attacker.name} has a damage buff: increasing damage to {finalDamage}");
    }

    targetStats.TakeDamage(finalDamage);

    // Show floating damage text.
    if (DamageTextSpawner.Instance != null)
    {
        DamageTextSpawner.Instance.ShowDamageText(target.transform.position, finalDamage);
    }

    // Create and fire a damage event.
    DamageInfo info = new DamageInfo(attacker, target, finalDamage, usedSkill, isCritical);
    OnDamageDealt?.Invoke(info);

    Debug.Log($"{attacker.name} dealt {(isCritical ? "CRITICAL " : "")}{finalDamage} damage to {target.name}");
}


    // Applies a damage over time effect to the target.
    public void ApplyDamageOverTime(GameObject target, int damagePerTick, float tickInterval, float duration)
    {
        if (target == null) return;
        DamageOverTime dot = target.GetComponent<DamageOverTime>();
        if (dot == null)
        {
            dot = target.AddComponent<DamageOverTime>();
        }
        dot.Setup(damagePerTick, tickInterval, duration);
    }
}


/// Container for damage event information.
public class DamageInfo
{
    public GameObject Attacker { get; }
    public GameObject Target { get; }
    public int Damage { get; }
    public Skill UsedSkill { get; }
    public bool IsCritical { get; }

    public DamageInfo(GameObject attacker, GameObject target, int damage, Skill usedSkill, bool isCritical)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
        UsedSkill = usedSkill;
        IsCritical = isCritical;
    }
}
