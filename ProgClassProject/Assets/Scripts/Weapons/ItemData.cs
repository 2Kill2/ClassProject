using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int dmg;
    public GameObject itemPrefab;
    public Sprite icon;

    public void Collect()
    {
        GameMaster gm = FindFirstObjectByType<GameMaster>();
        if (gm != null)
        {
            gm.AddItem(this);
            Debug.Log($"{itemName} collected and added to inventory.");
        }
        else
        {
            Debug.LogError("GameMaster not found! Cannot add item to inventory.");
        }
    }
}
