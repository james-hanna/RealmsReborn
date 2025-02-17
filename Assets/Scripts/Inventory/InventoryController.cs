using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public InventoryModel Model { get; private set; } = new InventoryModel();


    private void Awake()
    {
        Model.OnInventoryChanged += OnInventoryChanged;
    }

    private void Start()
{
    // For testing: add a dummy item.
    if (testItem != null)
    {
        Model.AddItem(testItem, 1);
        Model.AddItem(testItem2, 1);
        Debug.Log("Test items added to inventory.");
    }
    }
    public ItemData testItem;
    public ItemData testItem2;


    private void OnDestroy()
    {
        Model.OnInventoryChanged -= OnInventoryChanged;
    }

    private void OnInventoryChanged()
    {
        InventoryUIManager.Instance?.RefreshUI(Model);
    }

    public void AddItem(ItemData item, int quantity)
    {
        Model.AddItem(item, quantity);
    }

    public bool RemoveItem(ItemData item, int quantity)
    {
        return Model.RemoveItem(item, quantity);
    }
}
