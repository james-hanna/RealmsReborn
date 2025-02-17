using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    private int damagePerTick;
    private float tickInterval;
    private float duration;
    private float elapsedTime = 0f;
    private float lastTickTime = 0f;
    private CommonStats targetStats; 


    public void Setup(int damagePerTick, float tickInterval, float duration)
    {
        this.damagePerTick = damagePerTick;
        this.tickInterval = tickInterval;
        this.duration = duration;
        elapsedTime = 0f;
        lastTickTime = Time.time;
        targetStats = GetComponent<CommonStats>();
        if (targetStats == null)
        {
            Debug.LogWarning("DamageOverTime: No CommonStats found on target.");
        }
    }

    private void Update()
    {
        if (targetStats == null) return;

        elapsedTime += Time.deltaTime;
        if (Time.time - lastTickTime >= tickInterval)
        {
            targetStats.TakeDamage(damagePerTick);
            lastTickTime = Time.time;

            // Show floating damage text for each tick.
            if (DamageTextSpawner.Instance != null)
            {
                DamageTextSpawner.Instance.ShowDamageText(transform.position, damagePerTick);
            }
        }
        if (elapsedTime >= duration)
        {
            Destroy(this);
        }
    }
}
