using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Prefabs")]
    public Camera playerCamera;
    public GameObject playerPrefab;
    public GameObject menuCanvas;

    [Header("Settings")]
    public float moveSpeed = 3f;       // units per second
    public float rotationSpeed = 360f; // degrees per second
    private bool isMoving = false;     // prevent input while moving

    [Header("References")]
    public Room room; // current room player is in
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    // Grid-based player tracking
    public enum Facing { North, East, South, West }
    public Facing facing = Facing.North;

    void Start()
    {
        // Start in first room
        room = FindFirstObjectByType<Room>();
        transform.position = new Vector3(room.transform.position.x, transform.position.y, room.transform.position.z);
        targetPosition = transform.position;
        targetRotation = transform.rotation;

        room.EnterRoom();
    }

    void Update()
    {
        HandleInput();
        SmoothMoveAndRotate();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //toggle menu
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }

    void HandleInput()
    {
        if (isMoving) return; // prevent input during movement

        // Move forward
        if (Input.GetKeyDown(KeyCode.W))
        {
            Room nextRoom = GetRoomInFacingDirection();
            if (nextRoom != null)
                StartCoroutine(MoveToRoomSmooth(nextRoom));
            else
                Debug.Log("A wall blocks the way!");
        }

        // Move backward
        if (Input.GetKeyDown(KeyCode.S))
        {
            Room backRoom = GetRoomBehind();
            if (backRoom != null)
                StartCoroutine(MoveToRoomSmooth(backRoom));
            else
                Debug.Log("A wall blocks the way!");
        }

        // Rotate left
        if (Input.GetKeyDown(KeyCode.A))
        {
            RotateLeft();
        }

        // Rotate right
        if (Input.GetKeyDown(KeyCode.D))
        {
            RotateRight();
        }

        // Search current room
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (room != null)
            {
                Debug.Log("Searching current room...");
                room.RoomSearch();
            }
        }
    }

    // Smooth movement and rotation each frame
    void SmoothMoveAndRotate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Coroutine to move smoothly between rooms
    private IEnumerator MoveToRoomSmooth(Room targetRoom)
    {
        if (targetRoom == null) yield break;

        isMoving = true;

        targetPosition = new Vector3(targetRoom.transform.position.x, transform.position.y, targetRoom.transform.position.z);

        // Wait until we reach the target
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            yield return null;

        transform.position = targetPosition;
        room = targetRoom;
        room.EnterRoom();

        isMoving = false;
    }
    //Door active = WALL CLOSED
    //door inactive = OPEN PATH
    // Returns the room the player is facing
    private Room GetRoomInFacingDirection()
    {
        if (room == null) return null;

        switch (facing)
        {
            case Facing.North: return room.north != null && !room.NorthDoor.activeSelf ? room.north : null;
            case Facing.East: return room.east != null && !room.EastDoor.activeSelf ? room.east : null;
            case Facing.South: return room.south != null && !room.SouthDoor.activeSelf ? room.south : null;
            case Facing.West: return room.west != null && !room.WestDoor.activeSelf ? room.west : null;
        }
        return null;
    }

    // Returns the room behind the player
    private Room GetRoomBehind()
    {
        Facing opposite = Facing.North;
        switch (facing)
        {
            case Facing.North: opposite = Facing.South; break;
            case Facing.East: opposite = Facing.West; break;
            case Facing.South: opposite = Facing.North; break;
            case Facing.West: opposite = Facing.East; break;
        }

        switch (opposite)
        {
            case Facing.North: return room.north != null && !room.NorthDoor.activeSelf ? room.north : null;
            case Facing.East: return room.east != null && !room.EastDoor.activeSelf ? room.east : null;
            case Facing.South: return room.south != null && !room.SouthDoor.activeSelf ? room.south : null;
            case Facing.West: return room.west != null && !room.WestDoor.activeSelf ? room.west : null;
        }

        return null;
    }

    // Rotate player 90� left
    private void RotateLeft()
    {
        facing = (Facing)(((int)facing + 3) % 4); // wrap around 0-3
        targetRotation *= Quaternion.Euler(0, -90, 0);
    }

    // Rotate player 90� right
    private void RotateRight()
    {
        facing = (Facing)(((int)facing + 1) % 4);
        targetRotation *= Quaternion.Euler(0, 90, 0);
    }

    // Optional: spawn player in a given starting room
    public void SpawnPlayer(Room startingRoom)
    {
        if (startingRoom == null) return;

        room = startingRoom;
        transform.position = new Vector3(room.transform.position.x, transform.position.y, room.transform.position.z);
        targetPosition = transform.position;
        targetRotation = transform.rotation;

        room.EnterRoom();
    }
}
