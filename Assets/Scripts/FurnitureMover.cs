using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap), typeof(Collider2D))]
public class FurnitureMover : MonoBehaviour
{
    private Tilemap tilemap;
    private Grid grid;
    private Collider2D[] selfColliders;
    private bool isMoving = false;

    public float moveDuration = 0.4f;
    public Easing.Type easingType = Easing.Type.Linear;
    public int minMoveDistance = 1;
    public int maxMoveDistance = 3;

    private Vector3Int debugDir;
    private int debugDist;
    private bool debugCanMove;
    private bool debugHasData;
    private Collider2D debugBlockingCollider;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        grid = GetComponentInParent<Grid>();
        selfColliders = GetComponentsInChildren<Collider2D>();
    }

    public bool TryRandomMove()
    {

        if (isMoving)
            return false;


        Vector3Int[] directions =
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        System.Random rnd = new System.Random();
        directions = directions.OrderBy(x => rnd.Next()).ToArray();

        foreach (var dir in directions)
        {
            for (int dist = maxMoveDistance; dist >= minMoveDistance; dist--)
            {
                bool can = CanMove(dir, dist, out Collider2D blocking);

                debugDir = dir;
                debugDist = dist;
                debugCanMove = can;
                debugBlockingCollider = blocking;
                debugHasData = true;

                if (can)
                {
                    StartCoroutine(SmoothMove(dir, dist));
                    return true;
                }
            }
        }

        return false;
    }

    private bool CanMove(Vector3Int direction, int distance, out Collider2D blocking)
    {
        blocking = null;

        Bounds bounds = GetCombinedBounds(selfColliders);
        Vector3 cellOffset = grid.CellToWorld(direction * distance);

        // добавляем маленький буфер (0.05f, можно подстроить)
        Vector2 halfSize = (Vector2)bounds.extents - Vector2.one * 0.05f;
        Vector2 targetCenter = bounds.center + cellOffset;

        Collider2D[] hits = Physics2D.OverlapBoxAll(targetCenter, halfSize * 2f, 0f);
        foreach (var hit in hits)
        {
            if (hit == null || !hit.enabled) continue;
            if (selfColliders.Contains(hit)) continue;

            blocking = hit;
            return false;
        }

        // проверка буферной зоны (доп. зазор в 1 клетку)
        Vector3 bufferOffset = grid.CellToWorld(direction * (distance + 1));
        Vector2 bufferCenter = bounds.center + bufferOffset;
        hits = Physics2D.OverlapBoxAll(bufferCenter, halfSize * 2f, 0f);
        foreach (var hit in hits)
        {
            if (hit == null || !hit.enabled) continue;
            if (selfColliders.Contains(hit)) continue;

            blocking = hit;
            return false;
        }

        return true;
    }

    private Bounds GetCombinedBounds(Collider2D[] cols)
    {
        if (cols.Length == 0)
            return new Bounds(transform.position, Vector3.zero);

        Bounds result = cols[0].bounds;
        for (int i = 1; i < cols.Length; i++)
            result.Encapsulate(cols[i].bounds);
        return result;
    }

    private IEnumerator SmoothMove(Vector3Int direction, int distance)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + grid.CellToWorld(direction * distance);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            float eased = Easing.Apply(t, easingType);
            transform.position = Vector3.Lerp(startPos, targetPos, eased);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    private void OnDrawGizmos()
    {
        if (!debugHasData || grid == null)
            return;

        Gizmos.color = Color.white;
        var myColls = GetComponentsInChildren<Collider2D>();
        foreach (var col in myColls)
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);

        Vector3 worldOffset = grid.CellToWorld(debugDir * debugDist);
        Vector3 bufferOffset = grid.CellToWorld(debugDir * (debugDist + 1));

        Bounds totalBounds = GetCombinedBounds(myColls);
        Bounds target = totalBounds;
        target.center += worldOffset;
        Bounds buffer = totalBounds;
        buffer.center += bufferOffset;

        if (debugCanMove)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(target.center, target.size);
            Gizmos.color = new Color(0, 1, 1, 0.2f);
            Gizmos.DrawCube(target.center, target.size);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(target.center, target.size);
            Gizmos.color = new Color(1, 0, 0, 0.15f);
            Gizmos.DrawCube(target.center, target.size);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(buffer.center, buffer.size);

        if (debugBlockingCollider != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(debugBlockingCollider.bounds.center, debugBlockingCollider.bounds.size);
        }
    }
}

