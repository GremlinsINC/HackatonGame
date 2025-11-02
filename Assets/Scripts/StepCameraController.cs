using UnityEngine;

public class StepCameraController : MonoBehaviour
{
    public Transform player;
    public float roomWidth = 20f;
    public float roomHeight = 15f;
    public float moveSpeed = 5f;

    private Vector3 targetPosition;
    private int currentRoomX;
    private int currentRoomY;

    void Start()
    {
        UpdateCameraRoom(forceInstant: true);
    }

    void Update()
    {
        if (IsPlayerOutsideCurrentRoom())
        {
            UpdateCameraRoom(forceInstant: false);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    bool IsPlayerOutsideCurrentRoom()
    {
        float left = currentRoomX * roomWidth - roomWidth / 2f;
        float right = currentRoomX * roomWidth + roomWidth / 2f;
        float bottom = currentRoomY * roomHeight - roomHeight / 2f;
        float top = currentRoomY * roomHeight + roomHeight / 2f;

        return player.position.x < left || player.position.x > right ||
               player.position.y < bottom || player.position.y > top;
    }

    void UpdateCameraRoom(bool forceInstant = false)
    {
        currentRoomX = Mathf.RoundToInt(player.position.x / roomWidth);
        currentRoomY = Mathf.RoundToInt(player.position.y / roomHeight);

        float camX = currentRoomX * roomWidth;
        float camY = currentRoomY * roomHeight;

        targetPosition = new Vector3(camX, camY, transform.position.z);

        if (forceInstant)
            transform.position = targetPosition;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(currentRoomX * roomWidth, currentRoomY * roomHeight, 0), new Vector3(roomWidth, roomHeight, 0));
    }

}

