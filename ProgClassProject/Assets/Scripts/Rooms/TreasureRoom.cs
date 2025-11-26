using UnityEditor;
using UnityEngine;

public class TreasureRoom : MonoBehaviour
{
    [Header("Treasure Settings")]
    public Transform spawnPoint;
    private bool collected = false;
    private System.Random srng;
    private GameMaster gm;
    public ItemData[] possibleLoot;
    public ItemData treasureItem;
    

    public void Initialize(int seed, GameMaster gameMaster)
    {
        srng = new System.Random(seed);
        gm = gameMaster;
    }

    //called when player searches room
    public string SearchTreasure()
    {
        if (collected)
        {
            Debug.Log("The treasure has already been collected.");
            return "The treasure has already been collected.";
        }

        collected = true;

        if (possibleLoot.Length == 0 || possibleLoot == null)
        {
            Debug.LogWarning("No possible loot defined for this treasure room.");
            return "The treasure chest is empty.";
        }

        int lootIndex = srng.Next(0, possibleLoot.Length);
        ItemData loot = possibleLoot[lootIndex];
        
        string treasureName = loot.itemName;

        Instantiate(loot.itemPrefab, spawnPoint.position, Quaternion.identity);


        if (gm != null)
        {
            InventoryUIManager.Instance.AddItemToInventory(loot);
            gm.AddItem(loot);
            gm.ShowMessage($"You found a {treasureName}, {loot.dmg} dmg!");
        }
        return $"You found a {treasureName}, {loot.dmg} dmg!";
    }
}
