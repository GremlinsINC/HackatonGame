using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInventory : MonoBehaviour, IInventory
{
    public event System.Action<ItemData> OnItemAdded;

    private List<ItemData> items = new();

    public IReadOnlyList<ItemData> Items => items;

    public void AddItem(ItemData item)
    {
        items.Add(item);
        OnItemAdded?.Invoke(item);
    }
}

