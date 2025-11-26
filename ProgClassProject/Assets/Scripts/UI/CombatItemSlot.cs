using UnityEngine;
using UnityEngine.EventSystems;

public class CombatItemSlot : MonoBehaviour, IDropHandler
{
    public DraggableItem currentItem;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggedItem != null)
        {
            // Place the item in the slot
            draggedItem.transform.SetParent(transform);
            draggedItem.transform.localPosition = Vector3.zero;
            draggedItem.ParentAfterDrag = transform;
            currentItem = draggedItem;
        }
    }

    public int GetItemValue()
    {
        return currentItem != null ? currentItem.itemData.dmg : 0;
    }

    public void ClearSlot()
    {
        currentItem = null;
    }
}
