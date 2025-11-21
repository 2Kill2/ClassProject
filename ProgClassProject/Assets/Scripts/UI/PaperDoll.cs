using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DollSlot : MonoBehaviour, IDropHandler
{
    public ItemData currentItem;
    public Image slotImage;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggedItem != null && draggedItem.itemData != null)
        {
            currentItem = draggedItem.itemData;
            slotImage.sprite = currentItem.icon;
            slotImage.enabled = true;
        }
    }
}
