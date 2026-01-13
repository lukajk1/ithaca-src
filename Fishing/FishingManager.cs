using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [SerializeField] private bool printDebugStatements;

    [SerializeField] private AudioClip caughtFishSFX;
    private const float minBiteTime = 6f;
    private const float maxBiteTime = 12f;

    Dictionary<string, float> relativeRarity = new()
    {
        { "common", 100f },
        { "uncommon", 35f },
        { "rare", 8f },
        { "legendary", 0.1f },
        { "mythic", 0f },
    };

    Dictionary<string, float> relativeItemChance = new()
    {
        { "fish", 100f },
        { "scrap", 10f },
    };

    private FishData.Zone fishingZone;

    public static FishingManager i;

    private void Awake()
    {
        i = this;
    }

    public void Fish()
    {
        DayPeriod currentPeriod = DayPeriod.Afternoon;
        FishData.Zone zone = FishData.Zone.Saltwater;

        Rarity rolledRarity = RollWeightedRarity(zone);
        FishData caughtFish = FishDatabase.i.GetFishByZoneAndRarity(currentPeriod, zone, rolledRarity);

        // Fallback to old method if fish is null (shouldn't happen since we exclude empty pools)
        if (caughtFish == null)
        {
            caughtFish = FishDatabase.i.GetFish(currentPeriod, zone);
        }

        StartCoroutine(FishingWaitCR(caughtFish, NormalRandom()));
    }

    private Rarity RollWeightedRarity(FishData.Zone zone)
    {
        // Apply player stat modifiers to base rarity weights
        Dictionary<Rarity, float> weightedRarities = new()
        {
            { Rarity.Common, relativeRarity["common"] * PlayerInfo.Stats.commonWeightModifier },
            { Rarity.Uncommon, relativeRarity["uncommon"] * PlayerInfo.Stats.uncommonWeightModifier },
            { Rarity.Rare, relativeRarity["rare"] * PlayerInfo.Stats.rareWeightModifier },
            { Rarity.Legendary, relativeRarity["legendary"] * PlayerInfo.Stats.legendaryWeightModifier },
            { Rarity.Mythical, relativeRarity["mythic"] }
        };

        if (printDebugStatements)
        {
            Debug.Log($"common chance after modifier: {weightedRarities[Rarity.Common]}");
            Debug.Log($"uncommon chance after modifier: {weightedRarities[Rarity.Uncommon]}");
            Debug.Log($"rare chance after modifier: {weightedRarities[Rarity.Rare]}");
            Debug.Log($"legendary chance after modifier: {weightedRarities[Rarity.Legendary]}");
        }

        // Exclude rarities with no fish in this zone
        Dictionary<Rarity, float> availableRarities = new Dictionary<Rarity, float>();
        foreach (var kvp in weightedRarities)
        {
            if (FishDatabase.i.HasFishAtRarity(zone, kvp.Key))
            {
                availableRarities[kvp.Key] = kvp.Value;
            }
        }

        // Calculate total weight of available rarities
        float totalWeight = 0;
        foreach (var weight in availableRarities.Values)
        {
            totalWeight += weight;
        }

        // Roll random value and select rarity
        float random = Random.Range(0, totalWeight);
        float currentWeight = 0;

        foreach (var kvp in availableRarities)
        {
            currentWeight += kvp.Value;
            if (random < currentWeight)
            {
                if (printDebugStatements) Debug.Log($"picked {kvp.Key} rarity pool");
                return kvp.Key;
            }
        }

        // Fallback - return first available rarity
        foreach (var kvp in availableRarities)
        {
            return kvp.Key;
        }

        // Ultimate fallback to common (shouldn't reach here)
        return Rarity.Common;
    }

    private IEnumerator FishingWaitCR(FishData caughtFish, float quality)
    {
        float biteTime = Random.Range(minBiteTime, maxBiteTime) / PlayerInfo.Stats.biteSpeedModifier;
        // Debug.Log("bitetime" + biteTime);
        // Debug.Log("speedmod" + PlayerInfo.Stats.biteSpeedModifier);
        yield return new WaitForSeconds(biteTime);

        //Debug.Log($"Caught a {caughtFish.displayName}!");
        ConcreteItem concreteFish = new ConcreteItem(caughtFish, quality);

        PlayerInfo.TryAddToInventory(concreteFish);
        SoundManager.Play(new SoundData(caughtFishSFX, SoundData.Type.SFX, varyPitch: false, varyVolume: false));
        CaughtFishAlert.i.Display(concreteFish);
    }

    private float NormalRandom()
    {
        // Central limit theorem - averaging creates normal distribution
        float value = (Random.value + Random.value + Random.value) / 3f;
        value = Mathf.Clamp(value, 0.01f, 0.99f);
        return Mathf.Round(value * 100f) / 100f; // Round to 2 decimal places
    }
}