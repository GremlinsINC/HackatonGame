using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInventory : MonoBehaviour, IInventory, IMusicEntity
{
    public event System.Action OnItemAdded;

    private List<ItemData> items = new();

    public string Name => "Player";
    public Transform Transform => transform;

    public Music Music
    {
        get
        {
            HashSet<ItemData> uniqItems = new HashSet<ItemData>(items);
            List<AudioClip> all = new();

            foreach(var item in uniqItems)
            {
                int itemCount = items.Count(i => i.itemName == item.itemName);
                Debug.Log($"[PlayerInventory] {item.itemName} - {itemCount}");
                all.Add(item.music.layers[itemCount - 1]);
            }

            if(all.Count != 0)
            {
                Music music = new Music();
                music.instrumentName = "Mixed";
                music.layers = new AudioClip[]{AudioMixerUtility.MixClips(all)};
                return music;
            }else
                return null;

        }
    }

    public IReadOnlyList<ItemData> Items => items;

    public void AddItem(ItemData item)
    {
        Debug.Log("ITEM PICKED");
        items.Add(item);
        OnItemAdded?.Invoke();
    }
}

