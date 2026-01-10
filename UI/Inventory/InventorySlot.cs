using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem drag = dropped.GetComponent<DraggableItem>();

        // if this inventory slot already contains an item
        if (transform.childCount != 0)
        {
            transform.GetChild(0).SetParent(drag.parentTransform);
        }

        drag.parentTransform = transform;
    }
}
