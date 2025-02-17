using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : CommonStats
{
    [Header("Player Identity")]
    public string playerName = "Player";
    [Header("Mana")]
    public int maxMana = 100;
    [HideInInspector]
    public float currentMana = 100f;
    [Tooltip("Amount of mana restored per second.")]
    public float manaRegenRate = 5f;


    [Header("Experience")]
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    public UnityEvent OnLevelUp;

    protected override void Awake()
    {
        base.Awake();
        currentMana = maxMana;
        currentXP = 0;
    }

        private void Update()
    {
        RegenerateMana();
    }

    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana = Mathf.Min(currentMana + manaRegenRate * Time.deltaTime, maxMana);
        }
    }

    public void UseMana(int amount)
    {
        currentMana = Mathf.Max(currentMana - amount, 0);
        Debug.Log($"{gameObject.name} used {amount} mana. Mana: {currentMana}/{maxMana}");
    }

    public void GainXP(int xp)
    {
        currentXP += xp;
        Debug.Log($"{gameObject.name} gained {xp} XP. Total XP: {currentXP}/{xpToNextLevel}");
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        Debug.Log($"{gameObject.name} leveled up!");
        OnLevelUp?.Invoke();
        // increase max stats, etc.
    }
}
