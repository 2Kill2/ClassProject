using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Doors")]
    public GameObject NorthDoor, EastDoor, SouthDoor, WestDoor;

    [Header("Neighbors")]
    public Room north, east, south, west;

   
    public enum RoomType { Safe, Treasure, Encounter }
    public RoomType rType;

    private GameMaster gm;

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
                Debug.Log("You spot something!");
                int[] treasureDice = { 4, 6, 8, 20 };
                int newDice = treasureDice[Random.Range(0, treasureDice.Length)];
                gm.Inventory.Add(newDice);
                Debug.Log($"You found a d{newDice}!");
                return "You found treasure!";

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
        if (north != null && !isBorderNorth)
        {
            bool open = rng.Next(0, 2) == 0; // 50% chance
            NorthDoor.SetActive(open);
            if (north != null) north.SouthDoor.SetActive(open); // sync neighbor
            anyOpen |= open;
        }
        else if (north != null)
        {
            NorthDoor.SetActive(false);
            north.SouthDoor.SetActive(false);
        }

        // EAST
        if (east != null && !isBorderEast)
        {
            bool open = rng.Next(0, 2) == 0;
            EastDoor.SetActive(open);
            if (east != null) east.WestDoor.SetActive(open);
            anyOpen |= open;
        }
        else if (east != null)
        {
            EastDoor.SetActive(false);
            east.WestDoor.SetActive(false);
        }

        // SOUTH
        if (south != null && !isBorderSouth)
        {
            bool open = rng.Next(0, 2) == 0;
            SouthDoor.SetActive(open);
            if (south != null) south.NorthDoor.SetActive(open);
            anyOpen |= open;
        }
        else if (south != null)
        {
            SouthDoor.SetActive(false);
            south.NorthDoor.SetActive(false);
        }

        // WEST
        if (west != null && !isBorderWest)
        {
            bool open = rng.Next(0, 2) == 0;
            WestDoor.SetActive(open);
            if (west != null) west.EastDoor.SetActive(open);
            anyOpen |= open;
        }
        else if (west != null)
        {
            WestDoor.SetActive(false);
            west.EastDoor.SetActive(false);
        }

        // Ensure at least one door is open (so room isn’t isolated)
        if (!anyOpen)
        {
            if (north != null && !isBorderNorth)
            {
                NorthDoor.SetActive(true);
                if (north != null) north.SouthDoor.SetActive(true);
            }
            else if (east != null && !isBorderEast)
            {
                EastDoor.SetActive(true);
                if (east != null) east.WestDoor.SetActive(true);
            }
            else if (south != null && !isBorderSouth)
            {
                SouthDoor.SetActive(true);
                if (south != null) south.NorthDoor.SetActive(true);
            }
            else if (west != null && !isBorderWest)
            {
                WestDoor.SetActive(true);
                if (west != null) west.EastDoor.SetActive(true);
            }
        }
    }
}
