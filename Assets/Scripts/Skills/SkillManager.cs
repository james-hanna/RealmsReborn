using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private List<Skill> availableSkills;
    private Dictionary<Skill, float> lastUsedTime = new Dictionary<Skill, float>();

    private void Awake()
    {
        foreach (Skill skill in availableSkills)
        {
            lastUsedTime[skill] = -Mathf.Infinity;
        }
    }


    // Checks if a skill is ready based on its cooldown.

    public bool IsSkillReady(Skill skill)
    {
        if (lastUsedTime.TryGetValue(skill, out float lastTime))
        {
            return (Time.time - lastTime) >= skill.cooldown;
        }
        return true;
    }


    // Attempt to use a skill.

public void UseSkill(Skill skill, GameObject user, GameObject target)
{
    if (!IsSkillReady(skill))
    {
        Debug.Log($"{skill.skillName} is on cooldown.");
        return;
    }

    // Check if the user has mana
    PlayerStats ps = user.GetComponent<PlayerStats>();
    if (ps != null)
    {
        if (ps.currentMana < skill.manaCost)
        {
            Debug.Log($"{user.name} does not have enough mana to use {skill.skillName}.");
            return;
        }
        else
        {
            ps.UseMana((int)skill.manaCost);
        }
    }
    // For non-player characters, you might skip mana check

    // Execute the skill.
    skill.Execute(user, target);
    lastUsedTime[skill] = Time.time;
}


    /// Returns the remaining cooldown for a skill.
    public float GetCooldownRemaining(Skill skill)
    {
        if (lastUsedTime.TryGetValue(skill, out float lastTime))
        {
            float elapsed = Time.time - lastTime;
            return Mathf.Max(0, skill.cooldown - elapsed);
        }
        return 0;
    }


    /// Returns all available skills.
    public List<Skill> GetAvailableSkills() => availableSkills;
}
