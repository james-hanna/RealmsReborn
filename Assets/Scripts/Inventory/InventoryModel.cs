using System;
using System.Collections.Generic;

public class InventoryModel
{
    public class InventoryItem
    {
        public ItemData itemData;
        public int quantity;
    }

    public List<InventoryItem> Items { get; private set; } = new List<InventoryItem>();

    public event Action OnInventoryChanged;

    public void AddItem(ItemData item, int quantity)
    {
        var existing = Items.Find(i => i.itemData == item);
        if (existing != null)
            existing.quantity += quantity;
        else
            Items.Add(new InventoryItem { itemData = item, quantity = quantity });
        OnInventoryChanged?.Invoke();
    }

    public bool RemoveItem(ItemData item, int quantity)
    {
        var existing = Items.Find(i => i.itemData == item);
        if (existing != null && existing.quantity >= quantity)
        {
            existing.quantity -= quantity;
            if (existing.quantity <= 0)
                Items.Remove(existing);
            OnInventoryChanged?.Invoke();
            return true;
        }
        return false;
    }
}
