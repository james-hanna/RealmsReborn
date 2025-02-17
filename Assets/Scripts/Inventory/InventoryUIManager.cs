using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("Prefab for a single inventory slot (should have InventoryItemUI attached).")]
    public GameObject inventorySlotPrefab;
    [Tooltip("The parent transform for the grid (should have a GridLayoutGroup).")]
    public Transform gridParent;
    [Tooltip("Number of slots per page.")]
    public int slotsPerPage = 25;

    //private int currentPage = 0;

private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        Debug.Log("InventoryUIManager Instance set on " + gameObject.name);
    }
    else
    {
        Destroy(gameObject);
    }
}


    // Refreshes the UI grid from the inventory model.
    public void RefreshUI(InventoryModel inventory)
    {
         Debug.Log("Refreshing Inventory UI. Total items: " + inventory.Items.Count);
        // Clear the current grid.
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        int totalItems = inventory.Items.Count;
        for (int i = 0; i < slotsPerPage; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, gridParent);
            slot.name = "Slot_" + i;
            InventoryItemUI slotUI = slot.GetComponent<InventoryItemUI>();
            if (i < totalItems)
            {
                var invItem = inventory.Items[i];
                slotUI.Setup(invItem.itemData, invItem.quantity);
            }
            else
            {
                slotUI.Setup(null, 0);
            }
        }
    }
}
