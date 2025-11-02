using System.Collections.Generic;

public interface IInventory
{
    event System.Action OnItemAdded;
    void AddItem(ItemData item);
    IReadOnlyList<ItemData> Items { get; }
}

