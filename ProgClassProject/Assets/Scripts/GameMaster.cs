using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("References")]
    public MapMaster mapMaster;
    private Room roomMaster;
    public BattleMaster battleMaster;
    public PlayerController pc;
    public BackPackUI backpackUI;

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

    
    public List<ItemData> Inventory { get; set; } = new List<ItemData>();
    


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

    public void AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("Cannot add null item to inventory!");
            return;
        }

        Inventory.Add(item);
        Debug.Log($"Added {item.itemName} ({item.dmg} dmg) to inventory.");

        // Update Backpack UI
        if (backpackUI != null)
        {
            backpackUI.items = Inventory;
            backpackUI.PopulateBackpack();
        }
        else
        {
            Debug.LogWarning("BackpackUI reference is missing in GameMaster!");
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