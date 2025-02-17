using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance { get; private set; }
    
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 1f, 0);

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    public void ShowDamageText(Vector3 worldPosition, int damage)
    {
        if (damageTextPrefab == null || uiCanvas == null)
        {
            Debug.LogError("DamageTextSpawner: Missing prefab or canvas.");
            return;
        }
        
        Vector3 adjustedWorldPos = worldPosition + worldOffset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(adjustedWorldPos);
        
        GameObject dmgTextInstance = Instantiate(damageTextPrefab, screenPos, Quaternion.identity, uiCanvas.transform);
        FloatingDamageText floatingText = dmgTextInstance.GetComponent<FloatingDamageText>();
        if (floatingText != null)
        {
            floatingText.Setup(damage.ToString());
        }
    }
    
    public void ShowHealText(Vector3 worldPosition, int healAmount)
    {
        if (damageTextPrefab == null || uiCanvas == null)
        {
            Debug.LogError("DamageTextSpawner: Missing prefab or canvas.");
            return;
        }
        
        Vector3 adjustedWorldPos = worldPosition + worldOffset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(adjustedWorldPos);
        
        GameObject healTextInstance = Instantiate(damageTextPrefab, screenPos, Quaternion.identity, uiCanvas.transform);
        FloatingDamageText floatingText = healTextInstance.GetComponent<FloatingDamageText>();
        if (floatingText != null)
        {
            floatingText.Setup("+" + healAmount.ToString());
            // Set the text color to green for healing.
            floatingText.SetColor(Color.green);
        }
    }
}

