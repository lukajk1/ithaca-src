using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private TextMeshProUGUI microplasticsText;
    [SerializeField] private ItemGridManager gridManager;

    [Header("Equipment")]
    [SerializeField] private Transform fishingRodSlot;
    [SerializeField] private Transform lureSlot;
    [SerializeField] private Transform charmSlot;

    [SerializeField] private List<ItemData> debugInventoryList = new();

    public static InventoryManager i;
    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        gridManager.CreateGrid(PlayerInfo.Inventory.Length);

        for (int i = 0; i < debugInventoryList.Count; i++)
        {
            ConcreteItem item = new ConcreteItem(debugInventoryList[i]);
            PlayerInfo.TryAddToInventory(item);
        }
    }

    public void Open()
    {
        gridManager.CreateGrid(PlayerInfo.Inventory.Length);

        gridManager.PopulateGrid(PlayerInfo.Inventory);
        microplasticsText.text = $"{PlayerInfo.Microplastics}";
    }

    public void Close()
    {
        PlayerInfo.Inventory = gridManager.ExtractItemsFromGrid();
    }

    public void SetToFirstFreeSlot(GameObject tile)
    {
        gridManager.SetToFirstFreeSlot(tile);
    }

    public bool HasFreeSlot()
    {
        return gridManager.HasFreeSlot();
    }

    public void Equip(RectTransform oldParent, ItemTile tile, ConcreteItem item)
    {
        Transform targetSlot;

        // 1. Determine the target gear slot (Switch block for clean code)
        switch (item.data.type)
        {
            case ItemData.Type.FishingRod:
                targetSlot = fishingRodSlot;
                break;
            case ItemData.Type.Lure:
                targetSlot = lureSlot;
                break;
            case ItemData.Type.Charm:
                targetSlot = charmSlot;
                break;
            default:
                Debug.LogWarning($"Equip failed: Item type {item.data.type} is not an equipable type.");
                return;
        }

        // 2. Handle the currently equipped item (THE SWAP LOGIC)
        // Check if the target gear slot is already occupied
        ItemTile equippedGear = targetSlot.GetComponentInChildren<ItemTile>();

        if (equippedGear != null)
        {
            // Move the old equipped item back to the original inventory slot (oldParent)
            equippedGear.transform.SetParent(oldParent, false); // false maintains local scale/rotation
            equippedGear.isEquipped = false;

            // Reset the old item's RectTransform for the inventory slot
            RectTransform oldTileRect = equippedGear.GetComponent<RectTransform>();
            oldTileRect.localPosition = Vector3.zero;
            oldTileRect.localRotation = Quaternion.identity;
            oldTileRect.localScale = Vector3.one;
        }
        // NOTE: If no item was equipped (equippedGear == null), the new item's old slot (oldParent) 
        // will be empty, which the calling code (e.g., in ItemTile or InventoryManager) 
        // must handle to update the underlying data array (PlayerInfo.Inventory).

        // 3. Equip the new item
        tile.transform.SetParent(targetSlot, false);
        tile.isEquipped = true;

        // 4. Reset the new item's RectTransform for the gear slot
        RectTransform tileRect = tile.GetComponent<RectTransform>();
        tileRect.localPosition = Vector3.zero;
        tileRect.localRotation = Quaternion.identity;
        tileRect.localScale = Vector3.one;

        PlayerInfo.EquippedItems.Equip(item);
        PlayerInfo.CalculateStats();
    }
}
