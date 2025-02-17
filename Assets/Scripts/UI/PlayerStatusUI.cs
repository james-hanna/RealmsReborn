using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image healthBar;
    public Image manaBar;
    public Image xpBar;

    public Text healthText;
    public Text manaText;
    public Text xpText;
    
    [Header("Player Stats Reference")]
    public PlayerStats playerStats;

    private void Update()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStatusUI: PlayerStats reference not assigned.");
            return;
        }

        // Update Health Bar
        if (healthBar != null && playerStats.maxHealth > 0)
        {
            float healthRatio = (float)playerStats.currentHealth / playerStats.maxHealth;
            healthBar.fillAmount = Mathf.Clamp01(healthRatio);
        }
        if (healthText != null)
        {
            healthText.text = $"{playerStats.currentHealth} / {playerStats.maxHealth}";
        }
        
        // Update Mana Bar
        if (manaBar != null && playerStats.maxMana > 0)
        {
            float manaRatio = (float)playerStats.currentMana / playerStats.maxMana;
            manaBar.fillAmount = Mathf.Clamp01(manaRatio);
        }
        if (manaText != null)
        {
            manaText.text = $"{playerStats.currentMana} / {playerStats.maxMana}";
        }
        
        // Update XP Bar
        if (xpBar != null && playerStats.xpToNextLevel > 0)
        {
            float xpRatio = (float)playerStats.currentXP / playerStats.xpToNextLevel;
            xpBar.fillAmount = Mathf.Clamp01(xpRatio);
        }
        if (xpText != null)
        {
            xpText.text = $"{playerStats.currentXP} / {playerStats.xpToNextLevel}";
        }

    }
}
