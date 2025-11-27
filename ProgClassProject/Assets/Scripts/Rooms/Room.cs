using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    [Header("Doors")]
    public GameObject NorthDoor, EastDoor, SouthDoor, WestDoor;

    [Header("Neighbors")]
    public Room north, east, south, west;

   
    public enum RoomType { Safe, Treasure, Encounter }
    public RoomType rType;
    public TreasureRoom treasure;
    [SerializeField] public EncounterRoom encounter;

    private GameMaster gm;

    public bool CanMoveNorth => !NorthDoor.activeSelf && north != null;
    public bool CanMoveEast => !EastDoor.activeSelf && east != null;
    public bool CanMoveSouth => !SouthDoor.activeSelf && south != null;
    public bool CanMoveWest => !WestDoor.activeSelf && west != null;

    string resultMessage = "";
    void Start()
    {
        treasure = GetComponent<TreasureRoom>();
        encounter = GetComponent<EncounterRoom>();
    }

    // Called when player enters the room
    public virtual void EnterRoom()
    {
        Debug.Log($"Entering {name} ({rType})");
    }

    // Perform a search in the room
    public string RoomSearch()
    {
        switch (rType)
        {
            case RoomType.Safe:
                resultMessage = "The room is empty and safe.";
                gm.ShowMessage(resultMessage, 3f, 1f);
                break;

            case RoomType.Treasure:
                if (treasure != null)
                {
                    resultMessage = treasure.SearchTreasure();
                    if(gm != null && gm.NotificationText != null)
                    {
                        gm.ShowMessage(resultMessage, 3f, 1f); // message, display time, fade time
                    }
                }
                else
                {
                    resultMessage = "No treasure found.";
                    gm.ShowMessage(resultMessage, 3f, 1f);
                }
                break;

            case RoomType.Encounter:
                if(encounter != null)
                {
                    resultMessage = encounter.SearchEncounter();
                }
                else
                {
                    resultMessage = "No encounter found.";
                    gm?.ShowMessage(resultMessage, 3f, 1f);
                }
                break;

            default:
                resultMessage = "Nothing here.";
                break;
        }
        Debug.Log(resultMessage);
        return resultMessage;
    }

    // Set neighboring rooms and doors
  public void SetRooms(Room roomNorth, Room roomEast, Room roomSouth, Room roomWest, GameMaster gameMaster, Room fromRoom = null)
{
    gm = gameMaster;

    north = roomNorth;
    east = roomEast;
    south = roomSouth;
    west = roomWest;

    // Close all doors initially
    NorthDoor.SetActive(true);
    EastDoor.SetActive(true);
    SouthDoor.SetActive(true);
    WestDoor.SetActive(true);

    // Open doors for neighbors that exist
    if (north != null && north != fromRoom) NorthDoor.SetActive(true);
    if (east != null && east != fromRoom) EastDoor.SetActive(true);
    if (south != null && south != fromRoom) SouthDoor.SetActive(true);
    if (west != null && west != fromRoom) WestDoor.SetActive(true);

    // Randomize doors while ensuring back + forward path
    RandomizeDoors(fromRoom, north == null, east == null, south == null, west == null, new System.Random()); // pass your seed-based random here if you want determinism
}

// Randomly open doors while ensuring at least two accessible doors
// 'fromRoom' is the room we came from (to guarantee a back path)
public void RandomizeDoors(Room fromRoom, bool isBorderNorth, bool isBorderEast, bool isBorderSouth, bool isBorderWest, System.Random srng)
{
    // Start with all doors closed
    NorthDoor.SetActive(true);
    EastDoor.SetActive(true);
    SouthDoor.SetActive(true);
    WestDoor.SetActive(true);

    // 1. Keep the door back to the previous room open
    if (fromRoom != null)
    {
        if (fromRoom == north) { NorthDoor.SetActive(false); north.SouthDoor.SetActive(false); }
        else if (fromRoom == east) { EastDoor.SetActive(false); east.WestDoor.SetActive(false); }
        else if (fromRoom == south) { SouthDoor.SetActive(false); south.NorthDoor.SetActive(false); }
        else if (fromRoom == west) { WestDoor.SetActive(false); west.EastDoor.SetActive(false); }
    }

    // 2. Collect candidate doors leading to new rooms
    List<(Room, GameObject)> candidates = new List<(Room, GameObject)>();
    if (north != null && north != fromRoom && !isBorderNorth) candidates.Add((north, NorthDoor));
    if (east != null && east != fromRoom && !isBorderEast) candidates.Add((east, EastDoor));
    if (south != null && south != fromRoom && !isBorderSouth) candidates.Add((south, SouthDoor));
    if (west != null && west != fromRoom && !isBorderWest) candidates.Add((west, WestDoor));

    // 3. Guarantee at least one forward door
    if (candidates.Count > 0)
    {
        var chosen = candidates[srng.Next(0, candidates.Count)];
        chosen.Item2.SetActive(false); // open
        candidates.Remove(chosen); // remove from candidates so optional openings don't double-count
    }

    // 4. open other doors for branching
    foreach (var c in candidates)
    {
        if (srng.NextDouble() < 0.17) // 17% chance
        {
            c.Item2.SetActive(false);
        }
    }
}

}
