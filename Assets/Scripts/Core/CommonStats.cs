using UnityEngine;
using UnityEngine.Events;

public class CommonStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    [HideInInspector]
    public int currentHealth;

    [Header("Defense")]
    [Tooltip("Flat defense value to reduce incoming damage.")]
    public int defense = 0;

    public UnityEvent OnDeath; // Fires when health reaches zero.

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }


    /// Apply damage to the character. Damage is reduced by defense (but not below 0).
    public virtual void TakeDamage(int damage)
    {
        int effectiveDamage = Mathf.Max(damage - defense, 0);
        currentHealth -= effectiveDamage;
        Debug.Log($"{gameObject.name} took {effectiveDamage} damage. Health: {currentHealth}/{maxHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// Heals the character up to maxHealth.
    public virtual void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"{gameObject.name} healed for {amount}. Health: {currentHealth}/{maxHealth}");
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
