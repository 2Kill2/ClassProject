using UnityEngine;

public class MapMaster : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Room[] roomPrefabs;

    [Header("Settings")]
    [SerializeField] private float RoomSize = 1f;
    [SerializeField] private float RoomSpacing = 0.1f;

    [Header("References")]
    [SerializeField] private GridSettings _gridSettings = null;
    [SerializeField] private GameMaster gm;

    public Room[,] roomGrid;

    // Create the map and instantiate rooms
    public void CreateMap()
    {
        int rows = _gridSettings.GridSizeX;
        int cols = _gridSettings.GridSizeY;

        roomGrid = new Room[rows, cols];

        // Instantiate rooms
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int roll = Random.Range(0, roomPrefabs.Length);
                Room room = Instantiate(
                    roomPrefabs[roll],
                    new Vector3(c * (RoomSize + RoomSpacing), 0, r * (RoomSize + RoomSpacing)),
                    Quaternion.identity
                );
                room.name = $"Room_{r}_{c}";
                roomGrid[r, c] = room;
            }
        }

        // Set neighbors and doors
        SetRooms();
        Debug.Log("Map created with randomized rooms and doors.");
    }

    // Set neighbors for each room and randomize doors
    private void SetRooms()
    {
        int rows = _gridSettings.GridSizeX;
        int cols = _gridSettings.GridSizeY;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Room current = roomGrid[r, c];

                // Determine neighbors
                Room north = (r < rows - 1) ? roomGrid[r + 1, c] : null;
                Room south = (r > 0) ? roomGrid[r - 1, c] : null;
                Room east = (c < cols - 1) ? roomGrid[r, c + 1] : null;
                Room west = (c > 0) ? roomGrid[r, c - 1] : null;

                // Set neighbors in the room
                current.SetRooms(north, east, south, west, gm);

                // Determine if this room is on a border
                bool isBorderNorth = (r == rows - 1);
                bool isBorderSouth = (r == 0);
                bool isBorderEast = (c == cols - 1);
                bool isBorderWest = (c == 0);

                // Randomize doors while respecting borders
                current.RandomizeDoors(isBorderNorth, isBorderEast, isBorderSouth, isBorderWest);
            }
        }
    }
}
