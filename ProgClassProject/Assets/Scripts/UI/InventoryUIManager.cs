using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    [Header("Inventory UI References")]
    public Transform backpackGrid;
    public GameObject inventorySlotPrefab;

    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else if (Instance != this) 
        {
            Destroy(gameObject);
        }
    }

    public void AddItemToInventory(ItemData itemData)
    {
        GameObject newItem = Instantiate(inventorySlotPrefab, backpackGrid);

        DraggableItem draggableItem = newItem.GetComponent<DraggableItem>();
        draggableItem.Iniitialize(itemData);
        //draggableItem.ParentAfterDrag = backpackGrid;

        RectTransform rt = newItem.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;
    }
}
