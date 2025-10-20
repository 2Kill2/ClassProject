using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject treasureRoomPrefab;
    public GameObject safeRoomPrefab;
    public GameObject encounterRoomPrefab;

    [Header("Settings")]
    public int rows = 3;
    public int cols = 3;
    public float spacing = 2.0f;

    private GameObject playerInstance;
    private Room[,] map;
    private GameObject[,] roomInstances;

    public BattleMaster battleMaster;

    [Header("UI Elements")]
    public TMP_Text InventoryText;

    private (int row, int col) playerPos;

    void Start()
    {
        map = GenerateMap(rows, cols);
        CreateMapInstance();
        SpawnPlayer();
    }

    Room[,] GenerateMap(int rows, int cols)
    {
        Room[,] newMap = new Room[rows, cols];
        System.Random rng = new System.Random();

        for (int r = 0; r < rows; r++)
        {
            for (int c= 0; c < cols; c++)
            {
                int roll = rng.Next(0,3);
                Room room;
                
                switch (roll)
                {
                    case 0:
                        room = new TreasureRoom();
                        break;
                    case 1:
                        room = new SafeRoom();
                        break;
                    case 2:
                        room = new EncounterRoom();
                        break;
                    default:
                        room = new SafeRoom();
                        break;
                }

                newMap[r, c] = room;
            }
        }
        return newMap;
    }

    void CreateMapInstance()
    {
        roomInstances = new GameObject[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Room room = map[r, c];
                GameObject roomPrefab = room.GetRoomPrefab(treasureRoomPrefab, safeRoomPrefab, encounterRoomPrefab);

                if (room is TreasureRoom) roomPrefab = treasureRoomPrefab;
                else if (room is SafeRoom) roomPrefab = safeRoomPrefab;
                else if (room is EncounterRoom) roomPrefab = encounterRoomPrefab;

                Vector3 pos = new Vector3(c * 2, 0, r * 2);
                GameObject instance = Instantiate(roomPrefab, pos, Quaternion.identity);

                roomInstances[r, c] = instance;

                if (room is TreasureRoom)
                {
                    instance.GetComponent<Renderer>().material.color = Color.yellow;
                }
                else if (room is SafeRoom)
                {
                    instance.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (room is EncounterRoom)
                {
                    instance.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    void SpawnPlayer()
    {
        playerPos = (1, 1);
        playerInstance = Instantiate(playerPrefab, new Vector3(playerPos.col * 2, 1, playerPos.row * 2), Quaternion.identity);
    }

    private void Update()
    {
        //movement
        if (Input.GetKeyDown(KeyCode.W)) MovePlayer(-1, 0); //north
        if (Input.GetKeyDown(KeyCode.S)) MovePlayer(1, 0);  //south
        if (Input.GetKeyDown(KeyCode.A)) MovePlayer(0, 1); //west
        if (Input.GetKeyDown(KeyCode.D)) MovePlayer(0, -1);  //east

        //menus
        //if (Input.GetKeyDown(KeyCode.I)) OpenInventory();
        if (Input.GetKeyDown(KeyCode.F)) SearchRoom();
        if(Input.GetKeyDown(KeyCode.Escape)) QuitGame();
    }

    //controls
    public List<int> Inventory { get; set; } = new List<int>();
    /*public void OpenInventory()
    {
        Debug.Log("Inventory opened.");

        if (Inventory.Count == 0)
        {
            Debug.Log("Your pockets are empty.");
        }
        else
        {
            Debug.Log("You have the following items:");
            foreach (int d in Inventory)
                Debug.Log(d);
        }
    }*/

    public void UpdateInventoryUI()
    {
        if (Inventory.Count == 0)
        {
            InventoryText.text = "Inventory: Empty";
        }
        else
        {
            InventoryText.text = "Inventory:";
            foreach (int d in Inventory)
            {
                InventoryText.text += " " + d.ToString();
            }
        }
    }

    public void SearchRoom()
    {
        Room currentRoom = map[playerPos.row, playerPos.col];
        if (currentRoom is TreasureRoom)
        {
            Debug.Log("You found dice!");
            Inventory.Add(Random.Range(6, 20));
            UpdateInventoryUI();
        }
        else if (currentRoom is SafeRoom)
        {
            Debug.Log("Nothing fun here.");
        }
        else if (currentRoom is EncounterRoom)
        {
            Debug.Log("An enemy dicer!");
            battleMaster.StartEncounter(Inventory);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    void MovePlayer(int dRow, int dCol)
    {
        int newRow = playerPos.row + dRow;
        int newCol = playerPos.col + dCol;

        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
        {
            Debug.Log("You would go that way normally, but someone put a wall in the way.");
            return;
        }

        playerPos = (newRow, newCol);
        playerInstance.transform.position = new Vector3(newCol * 2, 1, newRow * 2);
        map[newRow, newCol].EnterRoom();
    }

    public virtual void EnterRoom()
    {
        Debug.Log("You enter a room.");
    }

}