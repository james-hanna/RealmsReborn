using UnityEngine;
using UnityEngine.UI;

public class SkillActivator : MonoBehaviour
{
    [Tooltip("The skill assigned to this button.")]
    public Skill assignedSkill;
    
    [Tooltip("Hotkey to activate this skill (optional).")]
    public KeyCode hotkey = KeyCode.None;

    private Button button;
    private SkillManager skillManager;
    private GameObject user;   

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnSkillButtonPressed);

        skillManager = FindObjectOfType<SkillManager>();
        if (skillManager == null)
            Debug.LogError("SkillManager not found in the scene!");

        user = GameObject.FindGameObjectWithTag("Player");
        if (user == null)
            Debug.LogError("Player not found! Please ensure the player has the tag 'Player'.");
    }

    private void Update()
    {
        if (hotkey != KeyCode.None && Input.GetKeyDown(hotkey))
        {
            OnSkillButtonPressed();
        }
    }

    public void OnSkillButtonPressed()
    {
        Debug.Log("Skill button pressed for skill: " + (assignedSkill != null ? assignedSkill.skillName : "None"));
        if (assignedSkill == null)
        {
            Debug.LogWarning("No skill assigned to this button.");
            return;
        }
        if (skillManager == null)
        {
            Debug.LogError("SkillManager not found, cannot activate skill.");
            return;
        }
        if (user == null)
        {
            Debug.LogError("User (Player) not found, cannot activate skill.");
            return;
        }

        GameObject skillTarget = null;
        if (assignedSkill.skillType == SkillType.SelfTarget)
        {
            // Self-target skills always target the user.
            skillTarget = user;
        }
        else
        {
            // For non-self skills, try to auto-target the nearest enemy.
            skillTarget = FindNearestEnemy();
            if (skillTarget == null)
            {
                Debug.LogWarning("No enemy target found for non-self skill.");
                return;
            }
        }

        skillManager.UseSkill(assignedSkill, user, skillTarget);
    }

    /// Finds the nearest enemy GameObject to the user.
    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            return null;

        GameObject nearest = enemies[0];
        float minDistance = Vector2.Distance(user.transform.position, nearest.transform.position);
        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(user.transform.position, enemy.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }
}
