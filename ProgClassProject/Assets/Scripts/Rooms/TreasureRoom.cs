using UnityEditor;
using UnityEngine;

public class TreasureRoom : MonoBehaviour
{
    [Header("Treasure Settings")]
    public GameObject[] treasurePrefabs;
    public Transform spawnPoint;
    private bool collected = false;
    private GameObject spawnedTreasure;

    private string treasureName;

    //called when player searches room
    public string SearchTreasure()
    {
        if (collected)
        {
            Debug.Log("The treasure has already been collected.");
            return "The treasure has already been collected.";
        }

        collected = true;

        //pick random treasure
        int index = Random.Range(0, treasurePrefabs.Length);
        GameObject treasure = treasurePrefabs[index];
        treasureName = treasure.name;

        //spawn treasure
        spawnedTreasure = Instantiate(treasure, spawnPoint.position, Quaternion.identity);
        Debug.Log($"You found a {treasureName}!");
        return $"You found a {treasureName}!";
    }
}
