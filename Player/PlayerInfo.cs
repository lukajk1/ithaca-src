using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // inventory ==================================================================
    private const int INITIAL_INVENTORY_SIZE = 15;

    public static int Microplastics = 450;
    public static ConcreteItem[] Inventory;

    public static ConcreteItem[] EquippedItems = new ConcreteItem[20];
    // yes this is not a good way to do it
    // 0 -> hat
    // 1 -> torso
    // 11 -> rod
    // 12 -> lure
    // 13 -> charm

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
        //Debug.Log("playinfo bite speed mod" + totalStats.biteSpeedModifier);
        // 2. Iterate through all slots in the EquippedItems array
        for (int i = 0; i < EquippedItems.Length; i++)
        {
            // Check if the slot is occupied by an item
            ConcreteItem equippedItem = EquippedItems[i];

            if (equippedItem != null && equippedItem.data is EquipmentData equipmentData)
            {
                // Retrieve the stats from the equipped item
                EquipmentStats itemStats = equipmentData.stats;

                // 3. AGGREGATE STATS
                // Since most stats are "modifiers" (multipliers), they should be added together.
                // Example: If Hat gives +0.1 and Torso gives +0.2, the total modifier is 1.3.
                totalStats.biteSpeedModifier += (itemStats.biteSpeedModifier - 1f);
                totalStats.castMinigameSpeedModifier += (itemStats.castMinigameSpeedModifier - 1f);
                totalStats.castStrengthModifier += (itemStats.castStrengthModifier - 1f);
                totalStats.castSpeedModifier += (itemStats.castSpeedModifier - 1f);
                totalStats.castWindowSizeModifier += (itemStats.castWindowSizeModifier - 1f);
                totalStats.minigameSpeedModifier += (itemStats.minigameSpeedModifier - 1f);
                totalStats.minigameDensityModifier += (itemStats.minigameDensityModifier - 1f);
                totalStats.commonWeightModifier += (itemStats.commonWeightModifier - 1f);
                totalStats.uncommonWeightModifier += (itemStats.uncommonWeightModifier - 1f);
                totalStats.rareWeightModifier += (itemStats.rareWeightModifier - 1f);
                totalStats.legendaryWeightModifier += (itemStats.legendaryWeightModifier - 1f);
                totalStats.rainyWeatherModifier += (itemStats.rainyWeatherModifier - 1f);
                totalStats.sunnyWeatherModifier += (itemStats.sunnyWeatherModifier - 1f);
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
