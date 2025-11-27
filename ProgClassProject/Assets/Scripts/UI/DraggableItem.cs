using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityEngine.UI.Image image;
    public ItemData itemData;
    private Transform originalParent;
    [HideInInspector] public Transform ParentAfterDrag;
    private Canvas canvas;

    void Awake()
    {
        if (image == null)
        {
            image = GetComponent<UnityEngine.UI.Image>();
        }

        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("DraggableItem must be a child of a Canvas.");
        }
    }
    public void Iniitialize(ItemData data)
    {
        itemData = data;
        UnityEngine.UI.Image img = GetComponent<UnityEngine.UI.Image>();
        if (img != null)
        {
            img.sprite = itemData.icon;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentAfterDrag);
        image.raycastTarget = true;
    }
}
