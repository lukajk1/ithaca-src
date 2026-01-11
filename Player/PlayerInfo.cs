using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentSlots
{
    public ConcreteItem Hat;
    public ConcreteItem Torso;
    public ConcreteItem FishingRod;
    public ConcreteItem Lure;
    public ConcreteItem Charm;

    public IEnumerable<ConcreteItem> GetAll()
    {
        yield return Hat;
        yield return Torso;
        yield return FishingRod;
        yield return Lure;
        yield return Charm;
    }

    public void Equip(ConcreteItem item)
    {
        switch(item.data.type)
        {
            case ItemData.Type.FishingRod:
                FishingRod = item;
                break;

            case ItemData.Type.Lure:
                Lure = item;
                break;

            case ItemData.Type.Charm:
                Charm = item;
                break;

            default:
                Debug.Log($"Item {item.data.displayName} could not be equipped (no matching type).");
                break;
            //case ItemData.Type.Hat:
            //    Hat = item;
            //    break;

            //case ItemData.Type.Torso:
            //    Torso = item;
            //    break;
        }
    }

    public void Unequip(ConcreteItem item)
    {
        switch(item.data.type)
        {
            case ItemData.Type.FishingRod:
                FishingRod = null;
                break;

            case ItemData.Type.Lure:
                Lure = null;
                break;

            case ItemData.Type.Charm:
                Charm = null;
                break;

            default:
                Debug.Log($"Item {item.data.displayName} could not be unequipped (no matching type).");
                break;
            //case ItemData.Type.Hat:
            //    Hat = null;
            //    break;

            //case ItemData.Type.Torso:
            //    Torso = null;
            //    break;
        }
    }
}

public class PlayerInfo : MonoBehaviour
{
    // inventory ==================================================================
    private const int INITIAL_INVENTORY_SIZE = 15;

    public static int Microplastics = 450;
    public static ConcreteItem[] Inventory;

    public static EquipmentSlots EquippedItems = new EquipmentSlots();

    public static EquipmentStats Stats;

    // region ==================================================================
    public static Region currentRegion = Region.Lowtown;
    private void Awake()
    {
        if (Inventory == null || Inventory.Length == 0)
        {
            Inventory = new ConcreteItem[INITIAL_INVENTORY_SIZE];
        }

        Stats = new EquipmentStats(useDefaults: true);
        //Debug.Log("playinfo bite speed mod" + Stats.biteSpeedModifier);
    }
    public static bool TryAddToInventory(ConcreteItem item)
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = item;
                return true;
            }
        }

        // else inventory full
        return false;
    }

    public static EquipmentStats CalculateStats()
    {
        // 1. Initialize the final stats with default values (1.0f for modifiers)
        EquipmentStats totalStats = new EquipmentStats(useDefaults: true);

        // 2. Iterate through all equipped items
        foreach (ConcreteItem equippedItem in EquippedItems.GetAll())
        {
            if (equippedItem != null && equippedItem.data is EquipmentData equipmentData)
            {
                // Retrieve the stats from the equipped item
                EquipmentStats itemStats = equipmentData.stats;

                // 3. AGGREGATE STATS
                // Since most stats are "modifiers" (multipliers), they should be added together.
                // Example: If Hat gives +0.1 and Torso gives +0.2, the total modifier is 1.3.
                totalStats.biteSpeedModifier += (itemStats.biteSpeedModifier);
                totalStats.castMinigameSpeedModifier += (itemStats.castMinigameSpeedModifier);
                totalStats.castStrengthModifier += (itemStats.castStrengthModifier);
                totalStats.castSpeedModifier += (itemStats.castSpeedModifier);
                totalStats.castWindowSizeModifier += (itemStats.castWindowSizeModifier);
                totalStats.minigameSpeedModifier += (itemStats.minigameSpeedModifier);
                totalStats.minigameDensityModifier += (itemStats.minigameDensityModifier);
                totalStats.commonWeightModifier += (itemStats.commonWeightModifier);
                totalStats.uncommonWeightModifier += (itemStats.uncommonWeightModifier);
                totalStats.rareWeightModifier += (itemStats.rareWeightModifier);
                totalStats.legendaryWeightModifier += (itemStats.legendaryWeightModifier);
                totalStats.rainyWeatherModifier += (itemStats.rainyWeatherModifier);
                totalStats.sunnyWeatherModifier += (itemStats.sunnyWeatherModifier);
            }
        }

        Stats = totalStats;
        return totalStats;
    }

    public static bool ResizeInventory(int newSize)
    {
        if (newSize <= Inventory.Length)
        {
            Debug.LogWarning($"Inventory resize failed: New size ({newSize}) must be larger than current size ({Inventory.Length}).");
            return false;
        }

        Array.Resize(ref Inventory, newSize);

        Debug.Log($"Inventory successfully resized to {Inventory.Length} slots.");
        return true;
    }

}
