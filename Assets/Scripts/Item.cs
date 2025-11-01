using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    public ItemData data;

    private SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInventory>(out var inventory))
        {
            inventory.AddItem(data);
            Destroy(gameObject);
        }
    }
    
    void OnValidate(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.icon;
    }
}
