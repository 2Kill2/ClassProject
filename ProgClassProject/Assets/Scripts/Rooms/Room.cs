using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject NorthDoor, EastDoor, SouthDoor, WestDoor;
    public Room north, east, south, west;

    GameMaster gm;
    public enum RoomType { safe, Treasure, Encounter }
    [SerializeField] public RoomType rType;

    public void Start()
    {
        //RandomizeDoors();
    }

    public virtual void EnterRoom()
    {
        Debug.Log($"Entering {name} ({rType})");
    }

    //this doesnt actually change when you move
    //it is reading every room is .safe
    //the player position is probably not being called go find where thats dealt and fix it

    //maybe read player position and compare it to what mapmaster spawned in that spot
    public string RoomSearch()
    {
        switch (rType)
        {
            case RoomType.safe:
                Debug.Log("Nothing here.");
                return "You found nothing here.";

            case RoomType.Treasure:
                Debug.Log("You spot something");
                int[] treasureDice = { 4, 6, 8, 20 }; //change this to whatever prefab spawns later
                System.Random rand = new System.Random();
                int newDice = treasureDice[rand.Next(treasureDice.Length)];
                gm.Inventory.Add(newDice);
                Debug.Log($"You found a d{newDice}!");
                return "You found something";

            case RoomType.Encounter:
                Debug.Log("You find a creature of some sort!");
                //battle start
                BattleMaster bm = FindFirstObjectByType<BattleMaster>();
                bm.StartEncounter(gm.Inventory);
                return "A battle!";

            default:
                Debug.Log("Nothing here.");
                return "Nothing here.";
        }
    }

    public void SetRooms(Room roomNorth, Room roomEast, Room roomSouth, Room roomWest, GameMaster gameMaster)
    {
        gm = gameMaster;
        north = roomNorth;
        NorthDoor.SetActive(north == null);
        east = roomEast;
        EastDoor.SetActive(east == null);
        south = roomSouth;
        SouthDoor.SetActive(south == null);
        west = roomWest;
        WestDoor.SetActive(west == null);
    }
    //this doesnt work
    //randomly create doors for the room, if there is a room in that direction, minumum of one door per room
        public void RandomizeDoors()
    {
        System.Random rng = new System.Random();
        bool doorCreated = false;
        // North Door
        if (north != null && rng.Next(0, 2) == 0)
        {
            NorthDoor.SetActive(true);
            doorCreated = true;
        }
        else
        {
            NorthDoor.SetActive(false);
        }
        // East Door
        if (east != null && rng.Next(0, 2) == 0)
        {
            EastDoor.SetActive(true);
            doorCreated = true;
        }
        else
        {
            EastDoor.SetActive(false);
        }
        // South Door
        if (south != null && rng.Next(0, 2) == 0)
        {
            SouthDoor.SetActive(true);
            doorCreated = true;
        }
        else
        {
            SouthDoor.SetActive(false);
        }
        // West Door
        if (west != null && rng.Next(0, 2) == 0)
        {
            WestDoor.SetActive(true);
            doorCreated = true;
        }
        else
        {
            WestDoor.SetActive(false);
        }
        // Ensure at least one door is created
        if (!doorCreated)
        {
            if (north != null) NorthDoor.SetActive(true);
            else if (east != null) EastDoor.SetActive(true);
            else if (south != null) SouthDoor.SetActive(true);
            else if (west != null) WestDoor.SetActive(true);
        }
    }


}
