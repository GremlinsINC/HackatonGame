using System.Collections.Generic;

public interface IInventory
{
    event System.Action<ItemData> OnItemAdded;
    void AddItem(ItemData item);
    IReadOnlyList<ItemData> Items { get; }
}

