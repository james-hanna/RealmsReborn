using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image iconImage;   
    public Text quantityText;  

    [HideInInspector]
    public ItemData itemData;
    [HideInInspector]
    public int quantity;

    private RectTransform rectTransform;
    private Canvas canvas;          
    private CanvasGroup canvasGroup; // For controlling raycasts and transparency during drag

    private Vector3 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }


    // Initializes the slot with an item and its quantity.
    public void Setup(ItemData data, int qty)
    {
        itemData = data;
        quantity = qty;
        if (iconImage != null)
            iconImage.sprite = data != null ? data.icon : null;
        if (quantityText != null)
            quantityText.text = qty > 0 ? qty.ToString() : "";
    }

    // Called when dragging starts.
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    // Called during dragging.
    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // Called when dragging ends.
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        // Snap back to original position (if not dropped into a valid target)
        rectTransform.anchoredPosition = originalPosition;
    }

    // Called when another draggable item is dropped on this slot.
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemUI sourceItem = eventData.pointerDrag.GetComponent<InventoryItemUI>();
        if (sourceItem != null && sourceItem != this)
        {
            SwapItems(sourceItem);
        }
    }

    // Swap the item data and quantity with another slot.
    private void SwapItems(InventoryItemUI other)
    {
        // Swap data and quantities
        ItemData tempData = itemData;
        int tempQty = quantity;

        itemData = other.itemData;
        quantity = other.quantity;

        other.itemData = tempData;
        other.quantity = tempQty;

        UpdateUI();
        other.UpdateUI();

    }

    // Updates the UI elements based on the current item data.
    public void UpdateUI()
    {
        if (iconImage != null)
            iconImage.sprite = itemData != null ? itemData.icon : null;
        if (quantityText != null)
            quantityText.text = quantity > 0 ? quantity.ToString() : "";
    }
}
