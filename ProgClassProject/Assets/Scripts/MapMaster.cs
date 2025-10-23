using UnityEngine;

public class MapMaster : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Room[] roomPrefabs;

    [Header("Settings")]
    [SerializeField] private float RoomSize = 1f;
    [SerializeField] private float RoomSpacing = 0.1f;
    [SerializeField] private float Rows = 3f;
    [SerializeField] private float cols = 3f;

    private Room room;
    //create map grid and populate with rooms randomly selected from roomPrefabs
    public void CreateMap()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int roll = Random.Range(0, roomPrefabs.Length);
                room = Instantiate(roomPrefabs[roll], new Vector3(c * (RoomSize + RoomSpacing), 0, r * (RoomSize + RoomSpacing)), Quaternion.identity);
                room.name = $"Room_{r}_{c}";
            }
        }
    }

}
