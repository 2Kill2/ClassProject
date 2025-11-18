using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Doors")]
    public GameObject NorthDoor, EastDoor, SouthDoor, WestDoor;

    [Header("Neighbors")]
    public Room north, east, south, west;

   
    public enum RoomType { Safe, Treasure, Encounter }
    public RoomType rType;
    public TreasureRoom treasure;

    private GameMaster gm;

    public bool CanMoveNorth => !NorthDoor.activeSelf && north != null;
    public bool CanMoveEast => !EastDoor.activeSelf && east != null;
    public bool CanMoveSouth => !SouthDoor.activeSelf && south != null;
    public bool CanMoveWest => !WestDoor.activeSelf && west != null;

    void Start()
    {
        treasure = GetComponent<TreasureRoom>();
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
                Debug.Log("Nothing here.");
                return "You found nothing here.";

            case RoomType.Treasure:
                if (treasure != null)
                {
                    return treasure.SearchTreasure();
                }
                else
                {
                    Debug.Log("No treasure component found!");
                    return "Error: No treasure found.";
                }

            case RoomType.Encounter:
                Debug.Log("You find a creature!");
                BattleMaster bm = FindFirstObjectByType<BattleMaster>();
                bm.StartEncounter(gm.Inventory);
                return "A battle starts!";

            default:
                return "Nothing here.";
        }
    }

    // Set neighboring rooms and doors
    public void SetRooms(Room roomNorth, Room roomEast, Room roomSouth, Room roomWest, GameMaster gameMaster)
    {
        gm = gameMaster;

        north = roomNorth;
        east = roomEast;
        south = roomSouth;
        west = roomWest;

        // DOOR LOGIC:
        // true = door exists / closed
        // false = no door / open / walkable
        NorthDoor.SetActive(north == null);
        EastDoor.SetActive(east == null);
        SouthDoor.SetActive(south == null);
        WestDoor.SetActive(west == null);
    }

    // Randomly open doors for this room
    // Border rooms will never open doors that lead outside
    public void RandomizeDoors(bool isBorderNorth, bool isBorderEast, bool isBorderSouth, bool isBorderWest)
    {
        bool anyOpen = false;

        System.Random rng = new System.Random();

        // NORTH
        if (north != null)
        {
            if (isBorderNorth)
            {
                NorthDoor.SetActive(true);
                north.SouthDoor.SetActive(true);
            }
            else
            {
                bool open = rng.Next(0, 2) == 0;
                NorthDoor.SetActive(!open); //open means not active
                north.SouthDoor.SetActive(!open);
                anyOpen |= open;
            }
        }
        else if (north != null)
        {
            NorthDoor.SetActive(false);
            north.SouthDoor.SetActive(false);
        }

        // EAST
         if (east != null)
    {
        if (isBorderEast)
        {
            EastDoor.SetActive(true);
            east.WestDoor.SetActive(true);
        }
        else
        {
            bool open = rng.Next(0, 2) == 0;
            EastDoor.SetActive(!open);
            east.WestDoor.SetActive(!open);
            anyOpen |= open;
        }
    }

        // SOUTH
          if (south != null)
    {
        if (isBorderSouth)
        {
            SouthDoor.SetActive(true);
            south.NorthDoor.SetActive(true);
        }
        else
        {
            bool open = rng.Next(0, 2) == 0;
            SouthDoor.SetActive(!open);
            south.NorthDoor.SetActive(!open);
            anyOpen |= open;
        }
    }

        // WEST
          if (west != null)
    {
        if (isBorderWest)
        {
            WestDoor.SetActive(true);
            west.EastDoor.SetActive(true);
        }
        else
        {
            bool open = rng.Next(0, 2) == 0;
            WestDoor.SetActive(!open);
            west.EastDoor.SetActive(!open);
            anyOpen |= open;
        }
    }

        // Ensure at least one door is open (so room isn�t isolated)
       if (!anyOpen)
    {
        if (north != null && !isBorderNorth)
        {
            NorthDoor.SetActive(false);        // open
            north.SouthDoor.SetActive(false);
        }
        else if (east != null && !isBorderEast)
        {
            EastDoor.SetActive(false);
            east.WestDoor.SetActive(false);
        }
        else if (south != null && !isBorderSouth)
        {
            SouthDoor.SetActive(false);
            south.NorthDoor.SetActive(false);
        }
        else if (west != null && !isBorderWest)
        {
            WestDoor.SetActive(false);
            west.EastDoor.SetActive(false);
        }
        }
    }
}
