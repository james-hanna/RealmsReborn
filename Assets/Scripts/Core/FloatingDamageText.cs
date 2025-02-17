using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    [Header("Animation Settings")]
    public float moveSpeed = 150f;
    public float fadeDuration = 1.5f;
    
    private Text damageText;
    private float elapsedTime = 0f;
    private Color originalColor;
    private RectTransform rectTransform;

    private void Awake()
    {
        damageText = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
        
        if (damageText != null)
            originalColor = damageText.color;
        else
            Debug.LogError("FloatingDamageText: No Text component found!");
    }

    public void Setup(string textValue)
    {
        if (damageText != null)
            damageText.text = textValue;
    }
    

    /// Allows change to text color.
    public void SetColor(Color newColor)
    {
        if (damageText != null)
        {
            damageText.color = newColor;
            originalColor = newColor;
        }
    }

    private void Update()
    {
        // Move text upward.
        if (rectTransform != null)
            rectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
        
        elapsedTime += Time.deltaTime;
        
        // Fade out the text.
        if (damageText != null)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / fadeDuration);
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }
        
        if (elapsedTime >= fadeDuration)
            Destroy(gameObject);
    }
}
