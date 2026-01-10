using UnityEngine;
using UnityEngine.EventSystems;

public class GearSlot : MonoBehaviour
{
    //[SerializeField] private GameObject gearIcon;
    //[SerializeField] private ItemData.Type type;

    //[SerializeField] private AudioClip equipClip;

    //private int childCount = 1;

    //private void OnTransformChildrenChanged()
    //{
    //    if (transform.childCount == 1)
    //    {
    //        gearIcon.SetActive(true);
    //    }
    //    else
    //    {
    //        gearIcon.SetActive(false);
    //    }
    //}

    //public void OnDrop(PointerEventData eventData)
    //{
    //    Debug.Log("drop");

    //    GameObject dropped = eventData.pointerDrag;
    //    ItemTile itemTile = dropped.GetComponent<ItemTile>();
    //    if (itemTile.item.data.type != type) return;

    //    ItemTile childTile = transform.GetComponentInChildren<ItemTile>();
    //    if (childTile != null && InventoryManager.FindFirstFreeSlot(out Transform tf))
    //    {
    //        childTile.transform.SetParent(tf);
    //    }

    //    DraggableItem drag = dropped.GetComponent<DraggableItem>();
    //    drag.parentTransform = transform;
    //}

    //void LateUpdate()
    //{
    //    if (transform.childCount != childCount)
    //    {
    //        childCount = transform.childCount;
    //        PlayEquip();
    //    }
    //}

    //public void PlayEquip()
    //{
    //    SoundManager.Play(new SoundData(equipClip, SoundData.Type.SFX));
    //}
}
