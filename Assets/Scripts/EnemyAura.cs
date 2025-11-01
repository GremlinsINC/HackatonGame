using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class EnemyAura : MonoBehaviour
{
    IReadOnlyList<GameObject> furnitures;

    public float moveDelay = 1f;

    void Start()
    {
        furnitures = SceneCacheManager.Instance.GetGroup("Tables");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(MoveFurniture());
        }
    }

    private IEnumerator MoveFurniture()
    {
        foreach(var gameObject in furnitures)
        {
            gameObject.TryGetComponent<FurnitureMover>(out var furnitureMover);
            furnitureMover.TryRandomMove();
            yield return new WaitForSeconds(moveDelay);
        }
    }
}
