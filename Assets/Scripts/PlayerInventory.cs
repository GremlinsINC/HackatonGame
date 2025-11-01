using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInventory : MonoBehaviour, IInventory, IMusicEntity
{
    public event System.Action<ItemData> OnItemAdded;

    private List<ItemData> items = new();

    public string Name => "Player";
    public Transform Transform => transform;

    public Music Music
    {
        get
        {
            return items.Count > 0 ? items[^1].music : null;
        }
    }

    public IReadOnlyList<ItemData> Items => items;

    public void AddItem(ItemData item)
    {
        items.Add(item);
        OnItemAdded?.Invoke(item);
    }
}

