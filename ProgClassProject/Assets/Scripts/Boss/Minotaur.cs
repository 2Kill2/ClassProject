using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// I asked chatGPT to help me with notes and ideas
public class Minotaur : MonoBehaviour
{
    [Header("References")]
    public Room currentRoom;            // The room the Minotaur is currently in
    public PlayerController player;     // Reference to the player

    [Header("Move settings")]
    public float moveSpeed = 2f;        // Units per second for smooth movement
    private bool isMoving = false;      // Tracks if the Minotaur is currently moving
    private Vector3 targetPosition;     // The target position to move toward in world space

    private void Start()
    {
        // Initialize the Minotaur's starting room if not already assigned
        if (currentRoom == null)
        {
            currentRoom = FindFirstObjectByType<Room>();
            Debug.Log($"Minotaur starting room not assigned, defaulting to {currentRoom.name}");
        }

        // Position the Minotaur at the center of its current room
        transform.position = currentRoom.transform.position;

        // Subscribe to the player's movement event so the Minotaur moves after the player
        if (player != null)
        {
            player.PlayerMoved += OnPlayerMove;
        }
        else
        {
            Debug.LogWarning("Player reference not assigned on Minotaur!");
        }
    }

    // Called whenever the player moves
    void OnPlayerMove()
    {
        Debug.Log("Minotaur: Player moved, attempting to move...");
        if(!isMoving)
        {
            StartCoroutine(MoveTowardsPlayer());
        }
        else
        {
            Debug.Log("Minotaur: Currently moving, skipping this turn.");
        }
    }

    // Coroutine to move the Minotaur smoothly towards the next room
    private IEnumerator MoveTowardsPlayer()
    {
        // Determine the next room to step into
        Room nextRoom = GetNextRoomTowardsPlayer();
        if (nextRoom == null)
        {
            Debug.Log("Minotaur: Failed to move - no path to player or blocked by walls.");
            yield break;
        }

        if (currentRoom == null)
        {
            Debug.LogError("Minotaur: Current room is null. Cannot move.");
            yield break;
        }

        // Update the current room reference
        currentRoom = nextRoom;

        // Set the target position in world space (keep the same Y to stay grounded)
        targetPosition = new Vector3(currentRoom.transform.position.x, transform.position.y, currentRoom.transform.position.z);
        isMoving = true;

        Debug.Log($"Minotaur: Moving to room {currentRoom.name} at position {targetPosition}");

        // Move smoothly towards the target using interpolation
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; // wait until next frame
        }

        // Snap to target to avoid tiny offsets
        transform.position = targetPosition;
        isMoving = false;

        Debug.Log($"Minotaur: Arrived at room {currentRoom.name}");

        // Check if the Minotaur caught the player
        if (currentRoom == player.room)
        {
            Debug.Log("Minotaur has caught the player!");
            // TODO: Implement game over logic here
        }
    }

    // Determine the next room towards the player using BFS pathfinding
    Room GetNextRoomTowardsPlayer()
    {
        if (currentRoom == null || player == null)
        {
            Debug.LogError("Minotaur: Cannot calculate path - currentRoom or player is null.");
            return null;
        }

        Queue<Room> queue = new Queue<Room>();
        Dictionary<Room, Room> cameFrom = new Dictionary<Room, Room>(); // Track paths

        queue.Enqueue(currentRoom);
        cameFrom[currentRoom] = null;

        while(queue.Count > 0)
        {
            Room room = queue.Dequeue();

            if (room == player.room)
            {
                // Backtrack to find the first step from current room
                Room step = room;
                Room prev = cameFrom[step];
                while (prev != currentRoom)
                {
                    step = prev;
                    prev = cameFrom[step];
                }
                return step; // Return the next room to move into
            }

            foreach (Room neighbor in GetOpenNeighbors(room))
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = room; // Track the path
                }
            }
        }

        Debug.Log("Minotaur: No path found to player.");
        return null; // No path found
    }

    // Returns a list of rooms that the Minotaur can move to from the given room
    List<Room> GetOpenNeighbors(Room room)
    {
        List<Room> neighbors = new List<Room>();

        if (room.CanMoveNorth) neighbors.Add(room.north);
        if (room.CanMoveEast) neighbors.Add(room.east);
        if (room.CanMoveSouth) neighbors.Add(room.south);
        if (room.CanMoveWest) neighbors.Add(room.west);

        return neighbors;
    }
}
