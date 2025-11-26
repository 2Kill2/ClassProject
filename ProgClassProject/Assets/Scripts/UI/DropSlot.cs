using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggedItem != null)
        {
            draggedItem.ParentAfterDrag = transform;
        }
    }
}
