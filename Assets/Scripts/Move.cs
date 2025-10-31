using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;

public class Move : MonoBehaviour
{
    [Header("Tilemap Settings")]
    public Tilemap obstacleTilemap;
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float moveDuration = 0.5f;
    public LayerMask obstacleLayer;
    public Easing.Type easingType = Easing.Type.Linear;
    
    private Vector3Int currentCell;
    private Coroutine moveCoroutine;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Grid grid;

    private readonly Dictionary<KeyCode, Vector2> _inputMap = new()
    {
        { KeyCode.UpArrow,    Vector2.up },
        { KeyCode.DownArrow,  Vector2.down },
        { KeyCode.LeftArrow,  Vector2.left },
        { KeyCode.RightArrow, Vector2.right }
    };

    void Start()
    {
        if (obstacleTilemap != null)
        {
            grid = obstacleTilemap.layoutGrid;
        }
        currentCell = obstacleTilemap.WorldToCell(transform.position);
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (isMoving)
            return;

        foreach (var pair in _inputMap)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                TryMove(pair.Value);
                break;
            }
        }
    }

    void TryMove(Vector2 direction)
    {
        Vector3 rayDirection = direction.normalized;
        Vector3 targetWorldPos = FindTargetPosition(rayDirection);
        Vector3Int targetCell = obstacleTilemap.WorldToCell(targetWorldPos);
        if(targetCell != currentCell){
            moveCoroutine = StartCoroutine(MoveToPosition(targetWorldPos));
            currentCell = targetCell;
        }
    }

    Vector3 FindTargetPosition(Vector3 direction)
    {
        Vector3Int dir = Vector3Int.RoundToInt(direction);
        Vector3Int cell = obstacleTilemap.WorldToCell(transform.position);

        for (int i = 0; i < 50; i++)
        {
            var next = cell + dir;
            if (obstacleTilemap.GetTile(next) != null) break;
            cell = next;
        }

        return obstacleTilemap.GetCellCenterWorld(cell);
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
      
        float elapsedTime = 0f;
        
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            float easedT = Easing.Apply(t, easingType);
            
            transform.position = Vector3.Lerp(startPosition, targetPosition, easedT);
            yield return null;
        }
        
        transform.position = targetPosition;
        isMoving = false;
    }
}
