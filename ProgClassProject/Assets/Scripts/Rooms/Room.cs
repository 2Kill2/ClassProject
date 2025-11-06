using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject NorthDoor, EastDoor, SouthDoor, WestDoor;
    private Room _north, _east, _south, _west;

    public enum RoomType { safe, Treasure, Encounter }
    public RoomType rType;

    public void Start()
    {
        RandomizeDoors();
    }

    public virtual void EnterRoom()
    {
        Debug.Log("You enter a room.");
    }

    public string RoomSearch()
    {
        GameMaster gm = FindFirstObjectByType<GameMaster>();
        switch (rType)
        {
            case RoomType.safe:
                Debug.Log("Nothing here.");
                return "You found nothing here.";

            case RoomType.Treasure:
                Debug.Log("You spot something");
                int[] treasureDice = { 4, 6, 8, 20 };
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

    public void SetRooms(Room roomNorth, Room roomEast, Room roomSouth, Room roomWest)
    {
        // Implementation for setting adjacent rooms can be added here
        _north = roomNorth;
        NorthDoor.SetActive(_north == null);
        _east = roomEast;
        EastDoor.SetActive(_east == null);
        _south = roomSouth;
        SouthDoor.SetActive(_south == null);
        _west = roomWest;
        WestDoor.SetActive(_west == null);
    }
    //this doesnt work
    //randomly create doors for the room, if there is a room in that direction, minumum of one door per room
        public void RandomizeDoors()
    {
        System.Random rng = new System.Random();
        bool doorCreated = false;
        // North Door
        if (_north != null && rng.Next(0, 2) == 0)
        {
            NorthDoor.SetActive(true);
            doorCreated = true;
        }
        else
        {
            NorthDoor.SetActive(false);
        }
        // East Door
        if (_east != null && rng.Next(0, 2) == 0)
        {
            EastDoor.SetActive(true);
            doorCreated = true;
        }
        else
        {
            EastDoor.SetActive(false);
        }
        // South Door
        if (_south != null && rng.Next(0, 2) == 0)
        {
            SouthDoor.SetActive(true);
            doorCreated = true;
        }
        else
        {
            SouthDoor.SetActive(false);
        }
        // West Door
        if (_west != null && rng.Next(0, 2) == 0)
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
            if (_north != null) NorthDoor.SetActive(true);
            else if (_east != null) EastDoor.SetActive(true);
            else if (_south != null) SouthDoor.SetActive(true);
            else if (_west != null) WestDoor.SetActive(true);
        }
    }


}
