using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class EnemyAura : MonoBehaviour
{
    List<GameObject> furnitures = new List<GameObject>();

    public float moveDelay = 1f;
    public string[] furnitureGroups;

    void Start()
    {
        foreach(var _group in furnitureGroups){
            furnitures.AddRange(SceneCacheManager.Instance.GetGroup(_group));
        }
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
