using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlotUI : MonoBehaviour, IDropHandler
{
    public Image iconImage;           
    public Text cooldownText;         
    public Image cooldownOverlay;     
    public Skill assignedSkill;
    
    private SkillManager skillManager;

    private void Awake()
    {
        skillManager = FindObjectOfType<SkillManager>();
    }

    private void Start()
    {
        if (assignedSkill != null)
        {
            SetSkill(assignedSkill);
        }
        else
        {
            Debug.LogWarning("SkillSlotUI: No assigned skill set on " + gameObject.name);
        }
    }

    public void SetSkill(Skill newSkill)
    {
        Debug.Log("Setting skill on " + gameObject.name + " with icon: " + newSkill.icon);
        assignedSkill = newSkill;
        if (iconImage != null && assignedSkill != null)
        {
            iconImage.sprite = assignedSkill.icon;
        }
        else
        {
            Debug.LogWarning("SkillSlotUI: IconImage or AssignedSkill is null on " + gameObject.name);
        }

        if (cooldownText != null)
        {
            cooldownText.text = "";
        }

        // Make sure the cooldown overlay is hidden initially.
        if (cooldownOverlay != null)
        {
            cooldownOverlay.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (assignedSkill != null && skillManager != null)
        {
            float remaining = skillManager.GetCooldownRemaining(assignedSkill);
            cooldownText.text = remaining > 0 ? remaining.ToString("F1") : "";

            // Update the overlay fill amount if a cooldown is active.
            if (cooldownOverlay != null)
            {
                if (remaining > 0)
                {
                    // Enable overlay if it's not active.
                    if (!cooldownOverlay.gameObject.activeSelf)
                        cooldownOverlay.gameObject.SetActive(true);

                    cooldownOverlay.fillAmount = remaining / assignedSkill.cooldown;
                }
                else
                {
                    cooldownOverlay.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Skill slot drop event");
        // Implement drag-and-drop assignment.
    }
}
