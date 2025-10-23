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
    public Camera mainCamera;

    [Header("Settings")]
    public int rows = 3;
    public int cols = 3;
    public float spacing = 2.0f;

    private GameObject playerInstance;
    private Room[,] map;
    private GameObject[,] roomInstances;

    [Header("UI Elements")]
    public TMP_Text InventoryText;

    [Header("References")]
    public MapMaster mapMaster;
    private Room roomMaster;
    public BattleMaster battleMaster;
    public PlayerController pc;

    //bool roomSearched = false;
    private (int row, int col) playerPos;

    void Start()
    {
        mapMaster.CreateMap();
        Debug.Log("Map created.");

        //spawn player
        pc.SpawnPlayer();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) QuitGame();
    }

    //controls
    public List<int> Inventory { get; set; } = new List<int>();
    
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


    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}