using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class MapMaster : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Room[] roomPrefabs;

    [Header("Settings")]
    [SerializeField] private float RoomSize = 1f;
    [SerializeField] private float RoomSpacing = 0.1f;
    //[SerializeField] private float Rows = 3f;
    //[SerializeField] private float cols = 3f;

    [Header("References")]
    [SerializeField] private GridSettings _gridSettings = null;
    public Room[,] roomGrid;
    [SerializeField] GameMaster gm;

    private Room room;
    //create map grid and populate with rooms randomly selected from roomPrefabs
    public void CreateMap()
    {
        roomGrid = new Room[_gridSettings.GridSizeX, _gridSettings.GridSizeY];

        for (int r = 0; r < _gridSettings.GridSizeX; r++)
        {
            for (int c = 0; c < _gridSettings.GridSizeY; c++)
            {
                int roll = Random.Range(0, roomPrefabs.Length);
                room = Instantiate(roomPrefabs[roll], new Vector3(c * (RoomSize + RoomSpacing), 0, r * (RoomSize + RoomSpacing)), Quaternion.identity);
                room.name = $"Room_{r}_{c}";
                roomGrid[r, c] = room;
            }
        }
        SetRooms();
    }
    //unity Z axis is r
    //unity x axis is c
    public void SetRooms()
    {
        for (int r = 0; r < _gridSettings.GridSizeX; r++)
        {
            for (int c = 0; c < _gridSettings.GridSizeY; c++)
            {
                var temproom = roomGrid[r, c];

                Room n, e, w, s;
                n = null;
                e = null;
                w = null;
                s = null;

                //r - 1 is south
                if (r > 0) s = roomGrid[r - 1, c];
                //r + 1 north
                if (r < _gridSettings.GridSizeX - 1) n = roomGrid[r + 1, c];
                //c - 1 west
                if (c > 0) w = roomGrid[r, c - 1];
                //c + 1 east
                if (c < _gridSettings.GridSizeY - 1) e = roomGrid[r, c + 1];

                temproom.SetRooms(n, e, w, s, gm);
            }
        }
    }

}
