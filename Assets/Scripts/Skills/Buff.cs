using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour
{
    public BuffType currentBuffType;
    public float buffAmount;
    public float buffDuration;

    /// Applies or refreshes a buff.

    public void ApplyBuff(BuffType type, float amount, float duration)
    {
        currentBuffType = type;
        buffAmount = amount;
        buffDuration = duration;
        StopAllCoroutines(); // If a buff of this type is already active, refresh it.
        StartCoroutine(BuffCoroutine());
    }

    private IEnumerator BuffCoroutine()
    {
        Debug.Log($"Buff applied: {currentBuffType} increased by {buffAmount} for {buffDuration} seconds.");
        yield return new WaitForSeconds(buffDuration);
        Debug.Log($"Buff ended: {currentBuffType}.");
        Destroy(this); // Remove the buff after the duration expires.
    }
}
