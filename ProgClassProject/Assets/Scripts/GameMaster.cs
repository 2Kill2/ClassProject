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
    public TMP_Text NotificationText;
    private Coroutine notifCoroutine;
    public List<string> InventoryItems = new List<string>();
    public Transform inventoryContent;
    public GameObject inventoryItemPrefab;

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
    }

    private void Update()
    {
    }

    //controls
    public List<int> Inventory { get; set; } = new List<int>();
    
    public void UpdateInventoryUI()
    {
        if (Inventory.Count == 0)
        {
            NotificationText.text = "Inventory: Empty";
        }
        else
        {
            NotificationText.text = "Inventory:";
            foreach (int d in Inventory)
            {
                NotificationText.text += " " + d.ToString();
            }
        }
    }

    public void ShowMessage(string message, float displayTime = 2f, float fadeTime = 1f)
    {
        if (notifCoroutine != null)
        {
            StopCoroutine(notifCoroutine);
        }

        NotificationText.text = message;
        NotificationText.alpha = 1f;
        notifCoroutine = StartCoroutine(FadeText(displayTime, fadeTime));
    }

    private System.Collections.IEnumerator FadeText(float displayTime, float fadeTime)
    {
        yield return new WaitForSeconds(displayTime);
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            NotificationText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            yield return null;
        }

        NotificationText.alpha = 0f;
        notifCoroutine = null;
    }

    public void AddItem(string itemName)
    {
        InventoryItems.Add(itemName);
        Debug.Log($"Added {itemName} to inventory.");
    }
    public void UpdateInventoryList()
    {
        if (inventoryContent == null || inventoryItemPrefab == null) return;

        // Clear previous UI entries
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // Populate UI with all items
        foreach (string itemName in InventoryItems)
        {
            GameObject itemEntry = Instantiate(inventoryItemPrefab, inventoryContent);
            TMP_Text textComponent = itemEntry.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = itemName;
            } 
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void SearchRoom()
    {
        Room currentRoom = map[playerPos.row, playerPos.col];
        //Debug.Log(currentRoom.RoomSearch());
    }

}