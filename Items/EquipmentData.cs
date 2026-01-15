using ExternPropertyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentItem", menuName = "Ithaca/Equipment")]
public class EquipmentData : ItemData
{
    public EquipmentStats stats;
}

[System.Serializable]
public struct EquipmentStats
{
    [InfoBox("Modifiers should not be prefixed with a '1'. i.e. 20% speed increase= 0.2, not 1.2. Modifiers on equipment are additive.")]
    [Space(10)]
    public float biteSpeedModifier;
    public float castStrengthModifier;

    [Header("Minigame Modifiers")]
    public float castSpeedModifier;
    public float castWindowSizeModifier;

    public float castMinigameSpeedModifier;
    public float minigameRotationSpeedModifier;

    [Tooltip("The number and spacing of obstacles")]
    public float minigameDensityModifier;

    [Header("Fishing Weight Bonuses")]
    public float commonWeightModifier;
    public float uncommonWeightModifier;
    public float rareWeightModifier;
    public float legendaryWeightModifier;

    [Header("Other Weight Bonuses")]
    public float rainyWeatherModifier;
    public float sunnyWeatherModifier;

    // default constructor. Used in the case that there are no equipped modifiers
    public EquipmentStats(bool useDefaults = true)
    {
        biteSpeedModifier = 1f;
        castMinigameSpeedModifier = 1f;

        minigameRotationSpeedModifier = 1f;
        castStrengthModifier = 1f;
        castSpeedModifier = 1f;
        castWindowSizeModifier = 1f;
        minigameDensityModifier = 1f;

        commonWeightModifier = 1f;
        uncommonWeightModifier = 1f;
        rareWeightModifier = 1f;
        legendaryWeightModifier = 1f;
        rainyWeatherModifier = 1f;
        sunnyWeatherModifier = 1f;
    }
}
