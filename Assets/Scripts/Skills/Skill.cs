using UnityEngine;

public enum SkillType
{
    SingleTarget,
    AOE,
    SelfTarget
}

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public abstract class Skill : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public SkillType skillType;
    public float cooldown;
    [Tooltip("For single-target skills, this is the maximum distance at which the skill can hit.")]
    public float range;
    [Tooltip("For AOE skills, this defines the radius of the effect.")]
    public float aoeRadius;
    [Tooltip("Damage (or healing) value. For healing skills, use negative damage.")]
    public int power;
    [Tooltip("Mana cost to use this skill.")]
    public float manaCost;  // NEW field
    
    public abstract void Execute(GameObject user, GameObject target);
}
