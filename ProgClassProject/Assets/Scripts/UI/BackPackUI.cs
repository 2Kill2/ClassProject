using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackPackUI : MonoBehaviour
{
    public Transform content;
    public GameObject itemPrefab;
    public List<ItemData> items = new List<ItemData>();

    public void PopulateBackpack()
    {
        if (content == null || itemPrefab == null) return;

        // Clear existing UI
        foreach (Transform child in content)
            Destroy(child.gameObject);

        // Populate with current items
        foreach (ItemData item in items)
        {
            GameObject entry = Instantiate(itemPrefab, content);
            Image img = entry.GetComponent<Image>();
            if (img != null && item.icon != null)
            {
                img.sprite = item.icon;
            }

            // Optional: assign DraggableItem component
            DraggableItem draggable = entry.GetComponent<DraggableItem>();
            if (draggable != null)
            {
                draggable.itemData = item;
            }
        }
    }
}
