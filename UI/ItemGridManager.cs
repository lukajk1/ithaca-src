using NUnit.Framework;
using StylizedWater3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGridManager : MonoBehaviour
{

    [SerializeField] private GameObject itemSlot;
    [SerializeField] private GameObject gridParent;
    [SerializeField] private GameObject itemTile;

    private List<GameObject> slotsList = new();
    public void CreateGrid(int slotCount)
    {
        // clear shop inventory of slots
        // remove children in reverse avoids issues with modifying list during iteration
        slotsList.Clear();

        for (int i = gridParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(gridParent.transform.GetChild(i).gameObject);
        }

        // ensure there are >= slots than items
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(itemSlot, gridParent.transform);
            slotsList.Add(slot);
        }

    }

    public void PopulateShopGrid(List<ItemData> items)
    {
        // populate with items
        for (int i = 0; i < items.Count; i++)
        {
            GameObject item = Instantiate(itemTile);

            ItemTile tile = item.GetComponent<ItemTile>();
            tile.Init(new ConcreteItem(items[i]));
            tile.isInShop = true;

            item.transform.SetParent(slotsList[i].transform, false);
        }
    }

    public void PopulateGrid(ConcreteItem[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) continue;

            GameObject item = Instantiate(itemTile);

            ItemTile tile = item.GetComponent<ItemTile>();
            tile.Init(items[i]);

            item.transform.SetParent(slotsList[i].transform, false);
        }
    }

    public bool SetToFirstFreeSlot(GameObject tile)
    {
        RectTransform tileRect = tile.GetComponent<RectTransform>();
        if (tileRect == null)
        {
            Debug.LogError("The tile GameObject does not have a RectTransform.");
            return false;
        }

        foreach (GameObject slot in slotsList)
        {
            if (slot != null && slot.transform.childCount == 0)
            {
                tileRect.SetParent(slot.transform);

                tileRect.localPosition = Vector3.zero;
                tileRect.localRotation = Quaternion.identity;
                tileRect.localScale = Vector3.one;

                return true;
            }
        }

        Debug.LogWarning("No free slot found.");
        return false;
    }

    public bool HasFreeSlot()
    {
        foreach (GameObject slot in slotsList)
        {
            if (slot != null && slot.transform.childCount == 0)
            {
                return true;
            }
        }

        return false;
    }

    public ConcreteItem[] ExtractItemsFromGrid()
    {
        ConcreteItem[] items = new ConcreteItem[slotsList.Count];

        for (int i = 0; i < slotsList.Count; i++)
        {
            if (slotsList[i] != null && slotsList[i].transform.childCount != 0)
            {
                ConcreteItem extracted = slotsList[i].transform.GetChild(0).GetComponent<ItemTile>().item;
                
                if (extracted != null)
                {
                    items[i] = extracted;
                }
            }
        }

        return items;
    }
}
